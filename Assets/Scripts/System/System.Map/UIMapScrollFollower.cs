using UnityEngine;

public class UIMapScrollFollower : MonoBehaviour
{
    [SerializeField] private RectTransform _marker;     // プレイヤーマーカー
    private RectTransform _targetCell;                  // 現在位置セル

    void Update()
    {
        if (_targetCell == null || _marker == null) return;

        // セルの座標をマーカーにコピー
        _marker.anchoredPosition = _targetCell.anchoredPosition;
    }

    /// <summary>
    /// マーカーが追いかけるCellをセット
    /// </summary>
    public void SetTarget(RectTransform cell)
    {
        _targetCell = cell;
    }
}
