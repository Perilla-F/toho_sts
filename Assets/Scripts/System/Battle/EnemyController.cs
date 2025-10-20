using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int _enemyId;
    private EnemyUIEventChannel _uiChannel;
    private EnemyData _data;

    public void Initialize(int enemyId, EnemyData data, EnemyUIEventChannel uiChannel)
    {
        _enemyId = enemyId;
        _data = data;
        _uiChannel = uiChannel;

        // 初期行動を登録など
    }

    public void PlanNextAction(Sprite intentIcon)
    {
        _uiChannel.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.ShowIntent,
            Icon = intentIcon
        });
    }

    public int GetEnemyId() => _enemyId;
    public void OnMouseOverTimelineIcon()
    {
        _uiChannel.Raise(new EnemyUIEventData
        {
            EnemyId = _enemyId,
            Type = EnemyUIEventType.Highlight
        });
    }
}
