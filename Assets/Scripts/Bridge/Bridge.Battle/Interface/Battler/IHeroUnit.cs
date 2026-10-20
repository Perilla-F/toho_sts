using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public interface IHeroUnit : IBattleUnit, IReadOnlyHeroUnit
{
    new List<StatusEffect> Effects { get; }
    IReadOnlyList<StatusEffect> IReadOnlyBattleUnit.Effects => Effects;
    new IMana Mana { get; }
    public void GainMana(int amount);
}

public interface IReadOnlyHeroUnit : IReadOnlyBattleUnit
{

    public Sprite PlayerEventIcon { get; }
    public IReadOnlyMana Mana { get; }
    public int DrawCount { get; }
}