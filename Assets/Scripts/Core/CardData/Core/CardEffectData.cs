[System.Serializable]
public class CardEffectData
{
    public CardEffectType type;
    public int value;
    public CardEffectTarget target; // "Self", "Enemy", "AllEnemies", etc
}