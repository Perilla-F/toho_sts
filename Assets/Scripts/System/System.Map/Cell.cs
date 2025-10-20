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
    public MultiStepEvent AssignedEvent;
    public CellBehavior Behavior;

    private Button _button;
    private bool IsSelectable;

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

        // クリック時にMapGeneratorへ通知
        _button.onClick.AddListener(OnClick);
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

    private void OnClick()
    {
        if (!IsSelectable) return;
        MapManager.Instance.SelectCell(this);

        Behavior.OnPlayerEnter();
    }
}
