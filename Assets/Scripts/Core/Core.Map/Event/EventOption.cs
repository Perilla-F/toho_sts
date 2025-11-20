using UnityEngine;

[System.Serializable]
public class EventOption
{
    public string Id;
    public string Text;

    [Header("進行")]
    public string DefaultNextStepId;
    public string ConditionalNextStepId;

    [Header("条件（任意）")]
    public EventCondition Condition;

    [Header("結果処理")]
    public EventResult Result;

    [Header("パラメータ（必要に応じて）")]
    public int HPChangeAmount;
    public int HPChangePer;
    public int GoldAmount;
    public string ItemId;
    public string FlagToSet;
    public string FlagToRemove;

    // 終了フラグ
    [Header("終了判定")]
    public bool EndsEvent;
}
