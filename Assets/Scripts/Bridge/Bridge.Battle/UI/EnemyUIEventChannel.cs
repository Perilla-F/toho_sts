using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyUIEventChannel
{
    public static Action<EnemyUIEventData> OnEventRaised;
    public static Action<NormalAction[]> OnEventPlaned;
}

[Serializable]
public class EnemyUIEventData
{
    public int EnemyId;
    public EnemyUIEventType Type;
    public Sprite Icon;
    public int Value;

    /// <summary>
    /// このターン中の何回目の自分の行動か
    /// </summary>
    public int Number;
}

public enum EnemyUIEventType
{
    /// <summary>
    /// イベントアイコンがハイライトされている
    /// </summary>
    Highlight,

    /// <summary>
    /// イベントアイコンがハイライトされていない
    /// </summary>
    Unhighlight,
}
