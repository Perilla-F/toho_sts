using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Enemy UI Event Channel")]
public class EnemyUIEventChannel : ScriptableObject
{
    public Action<EnemyUIEventData> OnEventRaised;

    public void Raise(EnemyUIEventData data)
    {
        OnEventRaised?.Invoke(data);
    }
}

[Serializable]
public class EnemyUIEventData
{
    public int EnemyId;
    public EnemyUIEventType Type;
    public Sprite Icon;
    public float Duration;
}

public enum EnemyUIEventType
{
    ShowIntent,   // 行動予定を表示
    Highlight,    // マウスオーバーなどのハイライト
    Unhighlight,  // ハイライト解除
    Damage,       // 被ダメージ表示など
}
