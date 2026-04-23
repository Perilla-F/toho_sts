using System.Threading.Tasks;

public class PlayerActionEvent : BattleEvent
{
    private ICardObj Card;

    public PlayerActionEvent(IHeroUnit hero, ICardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Hero = hero;
        Card = card;
        Type = EventType.Player;
    }

    public async override Task Execute(IBattleContext context)
    {
        await Card.Use();
    }
}