using Cysharp.Threading.Tasks;

public class PreviewActionEvent : BattleEvent
{
    public readonly IReadOnlyHeroUnit Hero;
    public readonly IReadOnlyCardObj Card;

    public PreviewActionEvent(IReadOnlyHeroUnit hero, IReadOnlyCardObj card, int scheduledTime)
        : base(scheduledTime, priority: 0)
    {
        Hero = hero;
        Card = card;
        Type = EventType.Preview;
    }

}