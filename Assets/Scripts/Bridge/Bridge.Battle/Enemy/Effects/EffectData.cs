using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class EffectData : ScriptableObject
{
    public string EffectId;      // 一意のID
    public string ClassName;     // 実装クラス名 (例: "PoisonEffect")
    public string DisplayName;   // 表示用
    public AssetReferenceSprite IconRef;
    [TextArea] public string Description;

    public bool isBuff;
}
