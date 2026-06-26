using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class BattleViewRoot : MonoBehaviour
{
    [SerializeField] public HandView HandView;
    [SerializeField] public DeckView DeckView;
    [SerializeField] public DiscardAreaView DiscardView;
    [SerializeField] public TimelineView TimelineView;
    [SerializeField] public ManaView ManaView;
    [SerializeField] public TurnMessagePanel TurnMessagePanel;


    public void Awake()
    {
        BattleEventBus.Battle.OnBattleStart += HandleBattleStart;
        BattleEventBus.Turn.OnTurnStart += HandleTurnStart;
        BattleEventBus.View.OnChangedManaCount += UpdateManaCount;
        BattleEventBus.View.OnChangedDeckCount += UpdateDeckCount;
        BattleEventBus.View.OnChangedDiscardCount += UpdateDiscardCount;
        BattleEventBus.Card.OnCardHovered += PreviewTimelineIcon;
    }

    private void HandleBattleStart(IReadOnlyBattleContext context, IMana mana)
    {
        BattleStart(context, mana).Forget();
    }

    private async UniTaskVoid BattleStart(IReadOnlyBattleContext context, IMana mana)
    {
        ManaView.Setup(mana);
        DeckView.Setup(context.Deck);
        DiscardView.Setup(context.Discard);
        await TurnMessagePanel.ShowMessage("戦闘開始");
    }

    private void HandleTurnStart(int turn, CancellationToken ct)
    {
        TurnStart(turn, ct).Forget();
    }

    private async UniTaskVoid TurnStart(int turn, CancellationToken ct)
    {
        await TurnMessagePanel.ShowMessage("第" + KanjiNumberConverteUtil.ConvertToKanjiWithUnits(turn) + "巡目");
    }

    private void UpdateManaCount() => ManaView.UpdateUI();

    private void UpdateDeckCount() => DeckView.UpdateDeckCount();

    private void UpdateDiscardCount() => DiscardView.UpdateDiscardCount();

    private void PreviewTimelineIcon(IReadOnlyCardObj card, IReadOnlyHeroUnit hero)
    {
        var previewEvent = new PreviewActionEvent(hero, hero.PlayerEventIcon, card, card.Source.Data.Delay);
        TimelineView.ShowPreview(previewEvent);
    }

    public void Oestroy()
    {
        BattleEventBus.Battle.OnBattleStart -= HandleBattleStart;
        BattleEventBus.Turn.OnTurnStart -= HandleTurnStart;
        BattleEventBus.View.OnChangedManaCount -= UpdateManaCount;
        BattleEventBus.View.OnChangedDeckCount -= UpdateDeckCount;
        BattleEventBus.View.OnChangedDiscardCount -= UpdateDiscardCount;
        BattleEventBus.Card.OnCardHovered -= PreviewTimelineIcon;
    }

}
