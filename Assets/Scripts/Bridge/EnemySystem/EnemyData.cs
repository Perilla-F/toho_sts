using UnityEngine;

public class EnemyData : IBattlerBaseData
{
    public string EnemyId;
    public string BattlerName { get; }
    public int MaxHP { get; }
    public int RewardGold;
    public EnemyAI EnemyAI;
    public EnemyType EnemyType;
}