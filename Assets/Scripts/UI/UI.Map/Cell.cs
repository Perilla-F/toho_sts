using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マップ上の1セルを表すクラス
/// </summary>
public class Cell : MonoBehaviour
{
    [Header("セル情報")]
    public CellType Type;
    public Vector2Int GridPos;
    public bool IsWide;

    [Header("UI")]
    public Image IconImage;
    public GameObject CurrentIcon;
    public GameObject SelectableEffect;

    [Header("状態管理")]
    public bool Cleared;

    private Button _button;
    public bool IsSelectable;

    public event Action<Vector2Int> OnClicked;

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
        _button = GetComponent<Button>();

        if (_button == null)
        {
            Debug.LogError("Cell に Button コンポーネントが必要です！");
            return;
        }

        Cleared = false;

        // クリック時にMapManagerへ通知
        _button.onClick.AddListener(OnMouseDown);
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

    private void OnMouseDown()
    {
        if (!IsSelectable) return;
        OnClicked?.Invoke(GridPos);
    }
}
