using System;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// マップ上の1セルを表すクラス
/// </summary>
public class CellUI : MonoBehaviour, IPointerClickHandler
{
    [Header("セル情報")]
    private CellType Type;
    public Vector2Int GridPos;
    public bool IsWide;

    [Header("UI")]
    [SerializeField] private Image IconImage;
    [SerializeField] private GameObject CurrentIcon;
    [SerializeField] private GameObject SelectableEffect;

    [Header("状態管理")]
    public bool Cleared;

    public bool IsSelectable;

    public event Action<Vector2Int, CellType> OnClicked;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(CellType type, Vector2Int gridPos, bool isWide)
    {
        Type = type;
        GridPos = gridPos;
        IsWide = isWide;

        if (CurrentIcon != null)
            CurrentIcon.SetActive(false);

        if (SelectableEffect != null)
            SelectableEffect.SetActive(false);
        IsSelectable = false;

        Cleared = false;
    }

    public void SetIcon(Sprite icon)
    {
        if (IconImage != null)
            IconImage.sprite = icon;
    }

    /// <summary>
    /// 現在選択セルかどうか
    /// </summary>
    public void SetCurrent(bool isCurrent)
    {
        if (CurrentIcon != null)
            CurrentIcon.SetActive(isCurrent);
    }

    /// <summary>
    /// 次に移動可能かどうか
    /// </summary>
    public void SetSelectable(bool selectable)
    {
        if (SelectableEffect != null)
        {
            SelectableEffect.SetActive(selectable);
            IsSelectable = selectable;
        }
    }

    /// <summary>
    /// 攻略済みにする
    /// </summary>
    public void MarkCleared()
    {
        Cleared = true;
    }

    public void OnPointerClick(PointerEventData e)
    {
        OnClicked?.Invoke(GridPos, Type);
    }

}
