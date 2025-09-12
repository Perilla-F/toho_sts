using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Effect Data")]
public class StatusEffectData : ScriptableObject
{
    public string effectId;      // 一意のID
    public string className;     // 実装クラス名 (例: "PoisonEffect")
    public string displayName;   // 表示用
    public Sprite icon;
    [TextArea] public string description;

    public int baseValue;
    public bool isBuff;
}
