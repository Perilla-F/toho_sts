using UnityEngine;

[System.Serializable]
public class EventOption
{
    public string Id;
    public string Text;

    [Header("分岐先ステップ")]
    public string DefaultNextStepId;   // 通常時の進行
    public string ConditionalNextStepId; // 条件成立時の進行

    [Header("条件（任意）")]
    public EventCondition Condition;

    [Header("結果処理")]
    public string FlagToSet;           // 実行後にONにするフラグ
    public string FlagToRemove;        // 実行後にOFFにするフラグ

    [Header("終了判定")]
    public bool EndsEvent;
}