using System;
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
    public IDeckView DeckView { get; private set; }
    public IHandView HandView { get; private set; }
    public IDiscardAreaView DiscardAreaView { get; private set; }
    public ITimelineView TimelineView;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    public UnityAction<CardBehavior> OnUse;
    public ICardObj Card;

    // State
    public CardSetUpState SetUpState { get; private set; }
    public CardIdleState IdleState { get; private set; }
    public CardWaitState WaitState { get; private set; }
    public CardDraggingState DraggingState { get; private set; }
    public CardSelectedState SelectedState { get; private set; }
    public CardStateBase CurrentState { get; private set; }

    public event Action<int> OnPointerCard;


    /// <summary>
    /// ハンド上のカードのデフォルトの重なり位置
    /// </summary>
    public int DefaultSiblingIndex;

    /// <summary>
    /// ハンド上のカードのデフォルトの位置
    /// </summary>
    private Vector2 _defaultPosition;

    public void Init(IDeckView deckView, IHandView handView, IDiscardAreaView discardAreaView, ITimelineView timelineView)
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

    public void BindCard(ICardObj card)
    {
        Card = card;
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
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }

    public void OnCardStateChange(ICardStateChangeEvent evt)
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
        CurrentState?.OnUpdate();
    }

    public void OnClick()
    {
        CurrentState?.OnClick();
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
        if (CurrentState == WaitState)
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

    public IEnumerator MoveTo(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }

    public async UniTask MoveToHandAsync()
    {
        transform.DOScale(Vector3.one, 0.3f);
        await transform.DOMove(HandView.GetTransform().position, 0.3f).AsyncWaitForCompletion();
    }

    public async UniTask MoveToDiscardAsync()
    {
        gameObject.SetActive(true);
        transform.SetParent(HandView.GetTransform(), worldPositionStays: true);

        // デッキ位置からスケール・回転付きで移動
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        transform.DORotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360);

        // 位置補正（HandView中央へ）
        await transform.DOMove(HandView.GetTransform().position, 0.3f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();
    }

}
