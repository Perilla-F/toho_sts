using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public CellType type;
    public CellBehaviour behaviour;
    public Vector2Int gridPos;
    public SpriteRenderer iconImage; // アイコンUI
    public GameObject currentIcon; // 現在地用アイコン
    public GameObject selectableEffect; // 選択可能エフェクト
    public bool isWide;

    public void Initialize(CellType cellType, Vector2Int pos, Sprite icon, bool wide = false)
    {
        type = cellType;
        gridPos = pos;
        iconImage.sprite = icon;
        isWide = wide;
        SetSelectable(false);
        SetCurrent(false);
    }

    private void Awake()
    {
        behaviour = GetComponent<CellBehaviour>();
    }

    public void SetSelectable(bool isOn)
    {
        if (selectableEffect != null)
            selectableEffect.SetActive(isOn);
    }

    public void SetCurrent(bool isOn)
    {
        if (currentIcon != null)
            currentIcon.SetActive(isOn);
    }
    void OnMouseDown()
    {
        OnClick(); // あなたが作った関数を手動で呼ぶ
    }

    public void OnClick()
    {
        Debug.Log($"Clicked: {gridPos}");
        if (MapGenerator.Instance.CanSelect(this))
        {
            MapGenerator.Instance.SelectCell(this);
        }
    }
}
