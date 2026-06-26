using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerActionEvent : BattleEvent
{
    public IHeroUnit Hero { get; set; }
    public readonly ICardObj Card;
    private readonly ICardContext cardContext;

    public PlayerActionEvent(IHeroUnit hero, ICardObj card, ICardContext cardContext, Sprite eventIcon, int scheduledTime)
        : base(eventIcon, scheduledTime, priority: 0)
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