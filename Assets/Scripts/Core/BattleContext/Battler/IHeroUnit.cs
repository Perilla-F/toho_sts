public interface IHeroUnit : IBattleUnit
{
    void GainMana(int amount);
    void Draw(int amount, IBattleContext context);
}