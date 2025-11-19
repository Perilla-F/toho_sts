using UnityEngine;

[CreateAssetMenu(menuName = "Game/Map/VisualSet")]
public class MapVisualSet : ScriptableObject
{
    [Header("セルタイプ別アイコン")]
    public Sprite BattleIcon;
    public Sprite EliteIcon;
    public Sprite BossIcon;
    public Sprite EventIcon;
    public Sprite RestIcon;
    public Sprite ShopIcon;
    public Sprite TreasureIcon;
    public Sprite StartIcon;
    public Sprite GoalIcon;

    [Header("Prefab設定")]
    public GameObject NormalCellPrefab;
    public GameObject WideCellPrefab;
}
