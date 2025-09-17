using System.Linq;

[System.Serializable]
public class ActionPattern
{
    public TurnActions[] entries;  // AttackActionなど
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
        for (int i = 0; i < entries.Length; i++)
        {
            cumulative += weights[i];
            if (roll < cumulative)
            {
                return CollectActionsFromEntries(entries[i]);
            }
        }
        return CollectActionsFromEntries(entries[0]);
    }

    /// <summary>
    /// ActionEntry[]からEnemyAction[]を抽出
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    public EnemyAction[] CollectActionsFromEntries(TurnActions entries)
    {
        EnemyAction[] actions = new EnemyAction[entries.actions.Length];
        for (var i = 0; i < entries.actions.Length; i++)
        {
            actions[i] = entries.actions[i].action;
        }
        return actions;
    }
}