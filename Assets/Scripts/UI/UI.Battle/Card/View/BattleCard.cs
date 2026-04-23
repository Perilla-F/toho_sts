using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using Cysharp.Threading.Tasks;


public class BattleCard : MonoBehaviour, IPoolableCard, ICardView, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private CardVisual visual;

    public GameObject GameObject => this.gameObject;
    public IDeckView DeckView { get; private set; }
    public IHandView HandView { get; private set; }
    public IDiscardAreaView DiscardAreaView { get; private set; }
    public UnityAction<BattleCard> OnUse;
    public ICardObj Card;

    private CardData data;

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

    public void Init(IDeckView deckView, IHandView handView, IDiscardAreaView discardAreaView)
    {
        DeckView = deckView;
        HandView = handView;
        DiscardAreaView = discardAreaView;

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
        data = card.Source.Data;
        SetupCardData(data);
    }

    public void SetupCardData(CardData data)
    {
        visual.UpdateVisual(data);
    }

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

    public async UniTask MoveToHandAsync()
    {
        await transform.DOScale(Vector3.one, 0.3f);
        await transform.DOMove(HandView.GetTransform().position, 0.3f).AsyncWaitForCompletion();
    }

    public async UniTask MoveToDiscardAsync()
    {
        gameObject.SetActive(true);
        transform.SetParent(HandView.GetTransform(), worldPositionStays: true);

        // デッキ位置からスケール・回転付きで移動
        transform.localScale = Vector3.zero;
        await transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        await transform.DORotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360);

        // 位置補正（HandView中央へ）
        await transform.DOMove(HandView.GetTransform().position, 0.3f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();
    }

    public void ResetForPool()
    {
        // 1. ステートをリセット（重要：残っていると再利用時にバグる）
        // もし可能なら ChangeState(null) や初期ステートへ

        // 2. DOTweenを完全に止める（再利用時に前のアニメーションが動いているのを防ぐ）
        transform.DOKill();

        // 3. Addressablesを解放
        visual.Cleanup();

        // 4. 親子関係を解除
        transform.SetParent(null);
    }
}
