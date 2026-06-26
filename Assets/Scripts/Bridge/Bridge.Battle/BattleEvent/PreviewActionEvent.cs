using Cysharp.Threading.Tasks;
using UnityEngine;

public class PreviewActionEvent : BattleEvent
{
    public readonly IReadOnlyHeroUnit Hero;
    public readonly IReadOnlyCardObj Card;

    public PreviewActionEvent(IReadOnlyHeroUnit hero, Sprite eventIcon, IReadOnlyCardObj card, int scheduledTime)
        : base(eventIcon, scheduledTime, priority: 0)
    {
        Hero = hero;
        Card = card;
        Type = EventType.Preview;
    }

}