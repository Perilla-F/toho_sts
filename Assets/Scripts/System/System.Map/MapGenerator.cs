using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;


    [Header("Cell Prefabs")]
    [SerializeField] private GameObject NormalCellPrefab;
    [SerializeField] private GameObject WideCellPrefab;

    [Header("Content")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _scrollRect;
    /// <summary>
    /// 画面縦の余白割合
    /// </summary>
    [SerializeField] private float verticalMarginPercent = 0.1f;
    /// <summary>
    /// 画面横の余白割合
    /// </summary>
    [SerializeField] private float horizontalMarginPercent = 0.05f;

    [Header("Icons")]
    [SerializeField] private Sprite StartIcon, GoalIcon, BattleIcon, EliteBattleIcon, BossBattleIcon, ShopIcon, TreasureIcon, RestIcon, EventIcon;

    private void Awake() => Instance = this;

    public void BuildUpUI(Dictionary<Vector2Int, MapCellState> mapData)
    {
        int Height = int.MinValue;
        int Width = int.MinValue;
        foreach (var pos in mapData.Keys)
        {
            if (pos.x > Width) Width = pos.x;
            if (pos.y > Height) Height = pos.y;
        }


        // Canvasサイズ取得
        float canvasHeight = _scrollRect.rect.height;
        float canvasWidth = _scrollRect.rect.width;

        // セルサイズ計算
        float cellSize = canvasWidth * (1 - horizontalMarginPercent * 2) / Width;

        // StartCellのYオフセット（下部余白）
        float bottomMargin = canvasHeight * verticalMarginPercent;
        float totalMapHeight = cellSize * Height;
        float startYOffset = -totalMapHeight / 2f + bottomMargin;

        foreach (var cell in mapData.Values)
        {
            Vector2Int pos;
            Sprite cellIcon;
            if (cell.Cell.IsWide)
            {
                pos = new(Width / 2, 0);
                cellIcon = GetIcon(cell.Cell.Type);
                BuildCell(WideCellPrefab, pos, cellIcon, startYOffset, cellSize, Width, true);
            }
            else
            {
                pos = cell.Cell.GridPos;
                cellIcon = GetIcon(cell.Cell.Type);
                BuildCell(NormalCellPrefab, pos, cellIcon, startYOffset, cellSize, Width, false);
            }
        }
        // Content の高さ調整
        AdjustContentSize(totalMapHeight, bottomMargin);
    }

    private void BuildCell(GameObject prefab, Vector2Int pos, Sprite icon, float yOffset, float size, int width, bool isWide = false)
    {
        GameObject obj = Instantiate(prefab, _content);
        RectTransform rt = obj.GetComponent<RectTransform>();

        // サイズを調整
        if (isWide)
        {
            rt.sizeDelta = new Vector2(size * 5f, size);
        }
        else
        {
            rt.sizeDelta = new Vector2(size, size);
        }

        // 座標を計算（中央揃え）
        float xOffset = size * (width - 1) / 2f;
        rt.anchoredPosition = new Vector2(pos.x * size - xOffset,
                                          pos.y * size + yOffset);
    }

    /// <summary>
    /// Content の高さをセル数に応じて調整
    /// </summary>
    void AdjustContentSize(float totalHeight, float margin)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, totalHeight + 2f * margin);
    }

    private Sprite GetIcon(CellType type) => type switch
    {
        CellType.Start => StartIcon,
        CellType.Goal => GoalIcon,
        CellType.Battle => BattleIcon,
        CellType.EliteBattle => EliteBattleIcon,
        CellType.BossBattle => BossBattleIcon,
        CellType.Shop => ShopIcon,
        CellType.Treasure => TreasureIcon,
        CellType.Rest => RestIcon,
        CellType.Event => EventIcon,
        _ => null
    };


}
