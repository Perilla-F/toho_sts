using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public CellType Type;
    public CellBehaviour Behaviour;
    public Vector2Int GridPos;
    public SpriteRenderer IconImage; // アイコンUI
    public GameObject CurrentIcon; // 現在地用アイコン
    public GameObject SelectableEffect; // 選択可能エフェクト
    public bool IsWide;
    public EventBase AssignedEvent;
    public EncounterData AssignedEncounter;

    public void Initialize(CellType cellType, Vector2Int pos, Sprite icon, bool wide = false)
    {
        Type = cellType;
        GridPos = pos;
        IconImage.sprite = icon;
        IsWide = wide;
        SetSelectable(false);
        SetCurrent(false);
    }

    private void Awake()
    {
        Behaviour = GetComponent<CellBehaviour>();
    }

    public void SetSelectable(bool isOn)
    {
        if (SelectableEffect != null)
            SelectableEffect.SetActive(isOn);
    }

    public void SetCurrent(bool isOn)
    {
        if (CurrentIcon != null)
            CurrentIcon.SetActive(isOn);
    }
    void OnMouseDown()
    {
        OnClick();
    }

    public void OnClick()
    {
        Debug.Log($"Clicked: {GridPos}");
        if (MapGenerator.Instance.CanSelect(this))
        {
            MapGenerator.Instance.SelectCell(this);
        }
    }
}
