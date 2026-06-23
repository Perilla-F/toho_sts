using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IHeroUnit : IBattleUnit, IReadOnlyHeroUnit
{
    new IMana Mana { get; }
    public void GainMana(int amount);
}

public interface IReadOnlyHeroUnit
{
    public Sprite PlayerEventIcon { get; }
    public IReadOnlyMana Mana { get; }
    public int DrawCount { get; }
}