public class PlayerActionEvent : BattleEvent
{
    private CardObj Card;

    public PlayerActionEvent(CardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Card = card;
    }

    public async override void Execute(IBattleContext context)
    {
        await Card.Use();
    }
}