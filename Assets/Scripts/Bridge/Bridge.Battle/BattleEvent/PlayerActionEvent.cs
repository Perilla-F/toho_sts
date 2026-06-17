using Cysharp.Threading.Tasks;

public class PlayerActionEvent : BattleEvent
{
    public readonly ICardObj Card;
    private readonly CardContext cardContext;

    public PlayerActionEvent(IHeroUnit hero, ICardObj card, CardContext cardContext, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Hero = hero;
        Card = card;
        this.cardContext = cardContext;
        Type = EventType.Player;
    }

    public async override UniTask Execute(IBattleContext context)
    {
        await Card.Use(cardContext);
    }
}