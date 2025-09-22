public abstract class IHeroUnit : BattleUnit
{
    public abstract void GainMana(int amount);
    public abstract void Draw(int amount, IBattleContext context);
}