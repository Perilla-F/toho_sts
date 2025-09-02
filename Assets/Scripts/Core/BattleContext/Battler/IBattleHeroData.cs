using UnityEngine;
public interface IBattleHeroData : IBattlerBaseData
{
    GameObject Live2DModelPrefab { get; set; }
    AnimationClip IdleMotionClip { get; set; }
}
