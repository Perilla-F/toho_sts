using UnityEngine;
public interface IBattlerBaseData
{
    string BattlerName { get; }
    int MaxHP { get; }
    RuntimeAnimatorController AnimatorController { get; }
}