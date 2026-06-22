using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class EventStep
{
    public string StepId;                       // ステップ識別子（例："start", "resultA"）
    [TextArea(2, 4)] public string Text;        // 表示テキスト
    public AssetReferenceSprite EventImageRef;  // ステップごとの画像（任意）

    public List<EventOption> Options;           // このステップで選べる選択肢
}