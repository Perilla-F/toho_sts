using UnityEngine;

[CreateAssetMenu(menuName = "Game/MapGenerationRule")]
public class MapGenerationRule : ScriptableObject
{
    [Header("マップ基本設定")]
    public int width = 5;
    public int height = 10;

    [Header("出現確率")]
    public float eliteRate = 0.1f;
    public float restRate = 0.15f;
    public float shopRate = 0.05f;

    [Header("マップの余白割合")]
    /// <summary>
    /// 画面縦の余白割合
    /// </summary>
    public float verticalMarginPercent = 0.1f;
    /// <summary>
    /// 画面横の余白割合
    /// </summary>
    public float horizontalMarginPercent = 0.05f;

}
