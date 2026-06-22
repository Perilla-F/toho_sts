using UnityEngine;
public interface IBattleHeroData
{
    GameObject Live2DModelPrefab { get; set; }
    AnimationClip IdleMotionClip { get; set; }
}
