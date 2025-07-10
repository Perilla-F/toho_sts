[System.Serializable]
public class EnemyActionData
{
    public EnemyActionType actionType;
    public int value;
    public int delay;
    public string targetId;
    public string parametersJson;
    public EnemyActionTarget target;
}
