using UnityEngine.UI;

public abstract class IHeroUnit : BattleUnit
{
    public Image playerPredictionIcon;
    public Image playerPreviewIcon;
    public Mana Mana;
    public int DrawCount;
    public abstract void GainMana(int amount);
    public abstract void Draw(int amount, IBattleContext context);
}