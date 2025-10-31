using System.Threading.Tasks;

public class PlayerActionEvent : BattleEvent
{
    private ICardObj Card;

    public PlayerActionEvent(ICardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Card = card;
    }

    public async override Task Execute(BattleContext context)
    {
        await Card.Use();
    }
}