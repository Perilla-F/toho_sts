using System.Linq;

[System.Serializable]
public class ActionPattern
{
    public TurnActions[] Pattern;  // AttackActionなど
    public int[] weights;

    /// <summary>
    /// ActionPatternからEnemyAction[]をランダム選出
    /// </summary>
    /// <returns></returns>
    public EnemyAction[] GetEnemyActions()
    {
        int total = 0;
        foreach (var w in weights) total += w;
        int roll = UnityEngine.Random.Range(0, total);
        int cumulative = 0;
        for (int i = 0; i < Pattern.Length; i++)
        {
            cumulative += weights[i];
            if (roll < cumulative)
            {
                return Pattern[i].Actions;
            }
        }
        return Pattern[0].Actions;
    }
}