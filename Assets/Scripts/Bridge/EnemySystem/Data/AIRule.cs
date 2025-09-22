using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/AIRule")]
public class AIRule : ScriptableObject
{
    public int priority;
    public Condition condition;
    public ActionRoutine routine;
}
