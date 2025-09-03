using System.Collections.Generic;

[System.Serializable]
public class EnemyConditionData
{
    public string ConditionName;

    public EnemyConditionType ConditionType;

    // HP 条件
    public float HpThresholdMin;
    public float HpThresholdMax;

    // ターン条件
    public int MaxTurn;

    // 状態異常条件
    public string RequiredPlayerEffect;

    // 行動パターン
    public List<EnemyTurnActions> Actions;

    public bool IsConditionMet(float hpPercent, int currentTurn, string playerEffects)
    {
        switch (ConditionType)
        {
            case EnemyConditionType.HP:
                return hpPercent >= HpThresholdMin && hpPercent <= HpThresholdMax;
            case EnemyConditionType.Turn:
                return currentTurn <= MaxTurn;
            case EnemyConditionType.PlayerEffect:
                return playerEffects.Contains(RequiredPlayerEffect);
            default:
                return false;
        }
    }
}
