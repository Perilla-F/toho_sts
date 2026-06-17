using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class IHeroUnit : BattleUnit
{
    public Sprite playerEventIcon;
    public Mana Mana;
    public int DrawCount;
    public abstract UniTask GainMana(int amount);
}