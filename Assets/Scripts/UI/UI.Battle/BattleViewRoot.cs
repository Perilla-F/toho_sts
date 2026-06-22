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
        BattleEventBus.Battle.OnBattleStart += (context, mana) => BattleStart(context, mana).Forget();
        BattleEventBus.Turn.OnTurnStart += (turn, ct) => TurnStart(turn, ct).Forget();
        BattleEventBus.View.OnChangedManaCount += UpdateManaCount;
        BattleEventBus.View.OnChangedDeckCount += UpdateDeckCount;
        BattleEventBus.View.OnChangedDiscardCount += UpdateDiscardCount;
        BattleEventBus.Card.OnCardHovered += PreviewTimelineIcon;
    }

    public async UniTaskVoid BattleStart(IReadOnlyBattleContext context, IMana mana)
    {
        ManaView.Setup(mana);
        DeckView.Setup(context.Deck);
        DiscardView.Setup(context.Discard);
        await TurnMessagePanel.ShowMessage("戦闘開始");
    }

    public async UniTaskVoid TurnStart(int turn, CancellationToken ct)
    {
        await TurnMessagePanel.ShowMessage("第" + turn + "巡目");
    }

    public void UpdateManaCount() => ManaView.UpdateUI();

    public void UpdateDeckCount() => DeckView.UpdateDeckCount();

    public void UpdateDiscardCount() => DiscardView.UpdateDiscardCount();

    private void PreviewTimelineIcon(IReadOnlyCardObj card, IReadOnlyHeroUnit hero)
    {
        var previewEvent = new PreviewActionEvent(hero, card, card.Source.Data.Delay);
        TimelineView.ShowPreview(previewEvent);
    }

    public void Oestroy()
    {
        BattleEventBus.Battle.OnBattleStart -= (context, mana) => BattleStart(context, mana).Forget();
        BattleEventBus.Turn.OnTurnStart -= (turn, ct) => TurnStart(turn, ct).Forget();
        BattleEventBus.View.OnChangedManaCount -= UpdateManaCount;
        BattleEventBus.View.OnChangedDeckCount -= UpdateDeckCount;
        BattleEventBus.View.OnChangedDiscardCount -= UpdateDiscardCount;
        BattleEventBus.Card.OnCardHovered -= PreviewTimelineIcon;
    }

}
