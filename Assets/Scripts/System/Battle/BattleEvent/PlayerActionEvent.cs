public class PlayerActionEvent : BattleEvent
{
    private ICardObj Card;

    public PlayerActionEvent(ICardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Card = card;
    }

    public async override void Execute(BattleContext context)
    {
        await Card.Use();
    }
}