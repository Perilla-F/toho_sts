using UnityEngine;

public abstract class IHeroUnit : BattleUnit
{
    public Sprite playerEventIcon;
    public Mana Mana;
    public int DrawCount;
    public abstract void GainMana(int amount);
}