using Live2D.Cubism.Framework.MotionFade;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyId;
    public string BattlerName;
    public Sprite EventIcon;
    public int MaxHP;
    public int RewardGold;
    public EnemyAIData EnemyAI;
    public EnemyType EnemyType;

    [SerializeField] public GameObject UIPrefab;    // HPバーなどのUIPrefab
    [SerializeField] public GameObject ModelPrefab;
    [SerializeField] public float ModelYOffset;

    [SerializeField] public AnimationClip IdleClip;
    [SerializeField] public AnimationClip AttackClip;
    [SerializeField] public AnimationClip HitClip;
    [SerializeField] public AnimationClip BuffClip;

}