using System.Linq;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TimelineIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private TextMeshProUGUI countDown;

    public BattleEvent BattleEvent;

    private int _enemyId;
    private RectTransform _rect;
    private CanvasGroup _canvasGroup;
    public int Count;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
    }

    public void Setup(BattleEvent battleEvent)
    {
        BattleEvent = battleEvent;
        switch (battleEvent)
        {
            case PlayerActionEvent pe:
                iconImage.sprite = pe.Hero.PlayerEventIcon;
                actionName.text = pe.Card.Source.Data.CardName;
                countDown.text = pe.Card.Delay.ToString();
                Count = pe.Card.Delay;
                break;
            case EnemyActionEvent ee:
                _enemyId = ee.EnemyId;
                iconImage.sprite = ee.Enemy.EventIcon;
                actionName.text = ee.ActionName;
                countDown.text = ee.action.ScheduledTime.ToString();
                Count = ee.action.ScheduledTime;
                break;
            case PreviewActionEvent pre:
                iconImage.sprite = pre.Hero.PlayerEventIcon;
                actionName.text = pre.Card.Source.Data.CardName;
                countDown.text = pre.Card.Delay.ToString();
                Count = pre.Card.Delay;
                break;
        }
    }

    public void SetStaticState()
    {
        var le = GetComponent<LayoutElement>();
        le.ignoreLayout = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
    }

    /// <summary>
    /// 生成時の演出用メソッド
    /// </summary>
    /// <param name="index"></param>
    public void PlaySpawnAnimation(int index)
    {
        var le = GetComponent<LayoutElement>();
        le.ignoreLayout = true;

        // 左から右へスライドしつつフェードイン
        _rect.anchoredPosition = new Vector2(-200f, _rect.anchoredPosition.y);
        _canvasGroup.alpha = 0f;

        _rect.DOAnchorPosX(0f, 0.5f)
            .SetDelay(index * 0.1f) // インデックス分だけ遅延させる
            .SetEase(Ease.OutBack);

        _canvasGroup.DOFade(1f, 0.5f)
            .SetDelay(index * 0.1f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            le.ignoreLayout = false;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
        });
    }

    /// <summary>
    /// 点滅演出
    /// </summary>
    public void StartBlinking()
    {
        var cg = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        cg.DOFade(0.3f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 確定演出
    /// </summary>
    public async UniTask PlayConfirmAnimation(CancellationToken ct)
    {
        var cg = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        var layout = GetComponent<LayoutElement>();

        // 1. 初期状態：透明、かつ高さ0
        cg.alpha = 0f;
        float targetHeight = layout.preferredHeight; // 元々の高さを記録
        layout.preferredHeight = 0f;

        // 2. Layoutを再計算（親に伝達）
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);

        // 3. アニメーション：高さを戻しながらフェードイン
        var fadeTask = cg.DOFade(1f, 0.3f).ToUniTask(cancellationToken: ct);
        var heightTask = DOTween.To(() => layout.preferredHeight, x => layout.preferredHeight = x, targetHeight, 0.3f)
                                .SetEase(Ease.OutBack)
                                .ToUniTask(cancellationToken: ct);

        await UniTask.WhenAll(fadeTask, heightTask);
    }

    public void UpdateCountDown(int count)
    {
        countDown.text = count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BattleEvent is EnemyActionEvent ee)
        {
            var actionData = ee.action;
            if (actionData is NormalAction na)
            {
                var tooltipData = na.effects.Select(e => new EffectDescription
                {
                    Name = e.Data.DisplayName,
                    Value = e.Amount.ToString(),
                    Description = e.Data.Description
                }).ToList();

                BattleTooltipManager.Instance.Show(tooltipData);
            }
        }
        else
        {
            return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleTooltipManager.Instance.Hide();
    }
}