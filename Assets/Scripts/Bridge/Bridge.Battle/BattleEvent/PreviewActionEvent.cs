using Cysharp.Threading.Tasks;

public class PreviewActionEvent : BattleEvent
{
    public readonly ICardObj Card;

    public PreviewActionEvent(IHeroUnit hero, ICardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Hero = hero;
        Card = card;
        Type = EventType.Preview;
    }

    public async override UniTask Execute(IBattleContext context)
    {
    }
}