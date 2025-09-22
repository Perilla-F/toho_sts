using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyId;
    public string BattlerName { get; }
    public int MaxHP { get; }
    public int RewardGold;
    public EnemyAIData EnemyAI;
    public EnemyType EnemyType;

    [SerializeField] public GameObject UIPrefab;    // HPバーなどのUIPrefab
    [SerializeField] public GameObject ModelPrefab;
    public float ModelYOffset;

    [SerializeField] public AnimationClip IdleClip;
    [SerializeField] public AnimationClip AttackClip;
    [SerializeField] public AnimationClip HitClip;
}