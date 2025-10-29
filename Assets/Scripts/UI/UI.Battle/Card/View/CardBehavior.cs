using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;


public class CardBehavior : MonoBehaviour, ICardView, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
{
    public DeckView DeckView { get; private set; }
    public HandView HandView { get; private set; }
    public DiscardAreaView DiscardAreaView { get; private set; }
    public TimelineView TimelineView;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    public UnityAction<CardBehavior> OnUse;

    // State
    public CardSetUpState SetUpState { get; private set; }
    public CardIdleState IdleState { get; private set; }
    public CardWaitState WaitState { get; private set; }
    public CardDraggingState DraggingState { get; private set; }
    public CardSelectedState SelectedState { get; private set; }
    public CardStateBase CurrentState { get; private set; }

    private CardStateBase _currentState;


    /// <summary>
    /// ハンド上のカードのデフォルトの重なり位置
    /// </summary>
    public int DefaultSiblingIndex;

    /// <summary>
    /// ハンド上のカードのデフォルトの位置
    /// </summary>
    private Vector2 _defaultPosition;

    public void Init(DeckView deckView, HandView handView, DiscardAreaView discardAreaView, TimelineView timelineView)
    {
        DeckView = deckView;
        HandView = handView;
        DiscardAreaView = discardAreaView;
        TimelineView = timelineView;
        // obj.BindMoveToHand(MoveToHandView);
        // obj.BindMoveToDiscard(MoveToDiscard);

        SetUpState = new CardSetUpState(this);
        IdleState = new CardIdleState(this);
        WaitState = new CardWaitState(this);
        DraggingState = new CardDraggingState(this);
        SelectedState = new CardSelectedState(this);

        ChangeState(SetUpState);
    }

    // void OnEnable()
    // {
    //     EventBus<CardStateChangeEvent>.Subscribe(OnCardStateChange);
    // }

    // void OnDisable()
    // {
    //     EventBus<CardStateChangeEvent>.Unsubscribe(OnCardStateChange);
    // }

    public void ChangeState(CardStateBase newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }

    void OnCardStateChange(ICardStateChangeEvent evt)
    {
        // if (evt.Source is CardObj card && card == CardObj)
        // {
        switch (evt.Source)
        {
            case CardStateName.CardSetUpState:
                ChangeState(SetUpState);
                break;
            case CardStateName.CardIdleState:
                ChangeState(IdleState);
                break;
            case CardStateName.CardWaitState:
                ChangeState(WaitState);
                break;
            case CardStateName.CardDraggingState:
                ChangeState(DraggingState);
                break;
            case CardStateName.CardSelectedState:
                ChangeState(SelectedState);
                break;
                // }
        }
    }

    public void Update()
    {
        _currentState?.OnUpdate();
    }

    public void OnClick()
    {
        _currentState?.OnClick();
    }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ResetPos()
    {
        transform.position = _defaultPosition;
        transform.SetSiblingIndex(DefaultSiblingIndex);
        transform.DOScale(Vector3.one, 0.1f);
        BezierArrows.Instance.Hide();
    }

    /// <summary>
    /// ドラッグ開始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentState == WaitState)
        {
            _defaultPosition = transform.position;
            ChangeState(DraggingState);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// マウスオーバー
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        CurrentState.OnPointerEnter(eventData);
    }

    /// <summary>
    /// マウスオーバー解除
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentState.OnPointerExit(eventData);
    }

    /// <summary>
    /// カードを手札に移動させる
    /// </summary>
    /// <returns></returns>
    private async UniTask MoveToHandView()
    {
        transform.gameObject.SetActive(true);
        // 拡大する
        transform.DOScale(Vector3.one, 0.3f);
        // 回転する
        transform.DORotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360);
        // handに移動する
        await transform.DOMove(HandView.transform.position, 0.3f).AsyncWaitForCompletion();
    }

    /// <summary>
    /// カードを墓場へ移動させる
    /// </summary>
    /// <returns></returns>
    public async UniTask MoveToDiscard()
    {
        // 縮小する
        transform.DOScale(Vector3.zero, 0.3f);
        // 回転する
        transform.DORotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360);
        await transform.DOMove(DiscardAreaView.transform.position, 0.3f).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }

    public async UniTask MoveToHandAsync()
    {
        transform.DOScale(Vector3.one, 0.3f);
        await transform.DOMove(HandView.transform.position, 0.3f).AsyncWaitForCompletion();
    }

    public async UniTask MoveToDiscardAsync()
    {
        transform.DOScale(Vector3.zero, 0.3f);
        await transform.DOMove(DiscardAreaView.transform.position, 0.3f).AsyncWaitForCompletion();
    }

}
