using System.Collections.Generic;

[System.Serializable]
public class EnemyActionData
{
    public string ActionName;
    public EnemyActionType ActionType;
    public int Value;
    public List<EffectEntry> Effects = new List<EffectEntry>();
    public int ScheduledTime;
    public int Delay;
    public int SimpleBlock;
    public EnemyActionTarget Target;
}
