public class PlayerActionEvent : BattleEvent
{
    private CardObj Card;
    private PlayerController _player;

    public PlayerActionEvent(CardObj card, PlayerController player, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Card = card;
        _player = player;
    }

    public async override void Execute(IBattleContext context)
    {
        await Card.Use();
    }
}