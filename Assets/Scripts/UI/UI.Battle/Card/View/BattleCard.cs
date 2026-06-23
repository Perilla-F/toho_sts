using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Reflection;

public class BattleCard : MonoBehaviour, IPoolableCard, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private CardVisual visual;
    [SerializeField] public float thresholdY;

    public GameObject GameObject => this.gameObject;
    public ICardObj Card;
    private IReadOnlyBattleContext _context;

    private CardData data;

    // State
    private CardStateMachine _stateMachine;

    private Canvas _canvas;

    private Vector2 _layoutPosition; // HandManagerから割り当てられた本来の位置
    private float _layoutRotation;   // HandManagerから割り当てられた本来の角度
    private int _defaultSiblingIndex;

    private Vector2 _dragStartPos; // ドラッグ開始時のスクリーン座標
    private const float ClickThreshold = 10f; // クリックとみなす移動距離のしきい値

    private CancellationToken _ct;

    private void Awake()
    {
        _stateMachine = new CardStateMachine(this);

        _stateMachine.ChangeState(new CardBusyState(this));

        _ct = this.GetCancellationTokenOnDestroy();
        BattleEventBus.Card.OnCardActive += CheckActive;
    }

    public void BindCard(ICardObj card, IReadOnlyBattleContext context)
    {
        Card = card;
        data = card.Source.Data;

        _context = context;

        _canvas = GetComponentInParent<Canvas>();

        SetupCardData(data);
    }

    public void SetupCardData(CardData data)
    {
        visual.UpdateVisual(data);
    }

    /// <summary>
    /// HandManagerが位置を確定させる時に呼ぶ
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="sibling"></param>
    public async UniTask SetLayoutPosition(Vector2 pos, float rot, int sibling)
    {
        _layoutPosition = pos;
        _layoutRotation = rot;
        _defaultSiblingIndex = sibling;

        var scaleTask = transform.DOScale(1f, 0.3f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: _ct);
        var MoveTask = transform.DOLocalMove(pos, 0.3f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: _ct);
        var RotateTask = transform.DOLocalRotate(new Vector3(0, 0, rot), 0.3f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: _ct);
        transform.SetSiblingIndex(sibling);
        await UniTask.WhenAll(scaleTask, MoveTask, RotateTask);
    }

    public void ResetPos()
    {
        transform.DOKill();
        transform.localRotation = Quaternion.Euler(0, 0, _layoutRotation);
        transform.DOLocalMove(_layoutPosition, 0.1f).SetEase(Ease.OutCubic);
        transform.DOLocalRotate(new Vector3(0, 0, _layoutRotation), 0.1f).SetEase(Ease.OutCubic);
        transform.SetSiblingIndex(_defaultSiblingIndex);
        transform.DOScale(Vector3.one, 0.1f);
        BezierArrows.Instance.Hide();
        if (EventSystem.current.currentSelectedGameObject == this.gameObject ||
        EventSystem.current.IsPointerOverGameObject() && _stateMachine.CurrentState is not CardTargetingState)
        {
            EventSystem.current.SetSelectedGameObject(null);
            CardHover();
        }
        else
        {
            HoverCancel();
        }
        BattleEventBus.Card.RestoreAllCards?.Invoke();
    }

    /// <summary>
    /// State切り替え
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(CardStateBase newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public void CheckActive(IReadOnlyCardObj cardObj)
    {
        if (Card != cardObj) ChangeState(new CardBusyState(this));
    }

    public void OnTryUseCard()
    {
        Debug.Log("Try Use!");
        if (!Card.Useable())
        {
            ResetPos();
            return;
        }
        if (Card.TargetType != CardEffectTarget.Enemy)
        {
            // 全体・自身ターゲットで、カードが中央（Targeting領域）にある場合
            ChangeState(new CardBusyState(this));
            BattleEventBus.Card.OnCardUsed?.Invoke(Card, null, _ct);
        }
        else if (CheckEnemyUnderMouse(out var enemy))
        {
            // 敵単体ターゲットの場合
            ChangeState(new CardBusyState(this));
            BattleEventBus.Card.OnCardUsed?.Invoke(Card, enemy, _ct);
        }
        else
        {
            // 失敗。手札に戻る
            ResetPos();
        }
    }

    private bool CheckEnemyUnderMouse(out IBattleUnit enemy)
    {
        enemy = null;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            var enemyModel = hit.collider.GetComponentInParent<EnemyModel>();
            if (enemyModel != null)
            {
                enemy = enemyModel.Self;
                return true;
            }
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            var enemyModel = result.gameObject.GetComponentInParent<EnemyModel>();
            if (enemyModel != null)
            {
                enemy = enemyModel.Self;
                return true;
            }
        }
        return false;
    }

    private void Update() => _stateMachine.Update();

    /// <summary>
    /// マウスオーバー
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_stateMachine.CurrentState is CardBusyState) return;
        _stateMachine.CurrentState.OnPointerEnter(eventData);
    }

    /// <summary>
    /// マウスオーバー解除
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_stateMachine.CurrentState is CardBusyState) return;
        _stateMachine.CurrentState.OnPointerExit(eventData);
    }

    /// <summary>
    /// ドラッグ開始
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 左クリック押下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_stateMachine.CurrentState.CanDrag) return;
        if (eventData.pointerId != -1) return;
        if (_stateMachine.CurrentState is CardTargetingState)
        {
            OnTryUseCard();
            return;
        }

        transform.DOKill();

        // 1. 最前面へ（ホバー時よりさらに上、Canvasレベルでの最前面へ）
        // 手札のCanvasGroupのブロックを避けるため、一時的に親を変える手法もありますが、
        // 単にSetAsLastSiblingでもCanvasが分かれていなければ大丈夫です。

        // 2. 縦になる（角度を0に）
        transform.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutCubic);
        transform.DOScale(Vector3.one * 1.0f, 0.15f); // ホバーの拡大をリセット

        // 開始時の座標を保存
        _dragStartPos = eventData.position;

        // 他のカードを非アクティブ化
        BattleEventBus.Card.OnCardActive?.Invoke(Card);

        // ドラッグ開始時のステートへ
        _stateMachine.ChangeState(new CardDraggingState(this));
    }

    /// <summary>
    /// 左クリックを離す
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_stateMachine.CurrentState is CardBusyState) return;
        if (_stateMachine.CurrentState is CardRestState) return;

        if (eventData.pointerId != -1) return;
        if (_stateMachine.CurrentState is CardTargetingState)
        {
            OnTryUseCard();
            return;
        }
        float dist = Vector2.Distance(_dragStartPos, eventData.position);
        if (dist < ClickThreshold)
            _stateMachine.ChangeState(new CardFollowingState(this));
        else
            ResetPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public Vector2 GetMouseCanvasPos()
    {
        // 1. 親となるCanvasのRectTransformを取得
        // (事前にキャッシュしておくと効率的です)
        RectTransform parentRect = transform.parent as RectTransform;

        // 2. マウスのスクリーン座標を取得
        Vector2 screenPos = Input.mousePosition;

        // 3. スクリーン座標をCanvas内のローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPos,
            _canvas.worldCamera, // RenderModeがOverlayならnullでもOK
            out var localPos);

        return localPos;
    }

    public void CardHover()
    {
        // 1. 最前列に出す
        transform.SetAsLastSibling();

        // 2. 外側に飛び出る（ローカルの上方向に移動）
        float hoverOffset = 30f; // 飛び出る距離
                                 // 自分の角度（度）をラジアンに変換して、移動ベクトルを計算
        float rad = (_layoutRotation + 90f) * Mathf.Deg2Rad; // 補正が必要な場合あり
        Vector2 moveDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector3 finalPos = (Vector3)_layoutPosition + (Vector3)moveDir * hoverOffset;

        // DOTweenで演出
        transform.DOKill(); // 動いている最中なら止める
        transform.DOLocalMove(finalPos, 0.15f).SetEase(Ease.OutCubic);
        transform.DOScale(1.1f, 0.15f); // 少し大きくするとさらに良い

        // タイムライン上にアイコンを載せる
        BattleEventBus.Card.OnCardHovered?.Invoke(Card, _context.Hero);
    }

    public void HoverCancel()
    {
        // 元の位置・角度・順序に戻す
        transform.DOKill();
        transform.DOLocalMove(_layoutPosition, 0.15f).SetEase(Ease.OutCubic);
        transform.DOScale(1f, 0.15f);
        transform.SetSiblingIndex(_defaultSiblingIndex); // 順番を戻す

        BattleEventBus.Card.OnCardExited?.Invoke();
    }

    /// <summary>
    /// カード使用時に中央へ動くアニメーション
    /// </summary>
    public async UniTask MoveToCenterAsync(CancellationToken ct)
    {
        transform.DOKill(); // 動いている最中なら止める
        var moveTask = transform.DOMove(new Vector2(0, 0), 0.15f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: ct);
        var scaleTask = transform.DOScale(2.0f, 0.15f).ToUniTask(cancellationToken: ct);
        await UniTask.WhenAll(moveTask, scaleTask);
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

    private void OnDestroy()
    {
        BattleEventBus.Card.OnCardActive -= CheckActive;
    }
}
