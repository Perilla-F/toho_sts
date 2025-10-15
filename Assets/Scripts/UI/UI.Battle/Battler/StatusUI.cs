using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    private BattleUnit _self;
    public StatusIcon StatusIconPrefab;
    public Transform[] rows;

    private ObjectPool<StatusIcon> iconPool;

    void Start()
    {
        iconPool = new ObjectPool<StatusIcon>(StatusIconPrefab, 16, transform);
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        // 一旦全アイコンを非表示に戻す
        foreach (var icon in GetComponentsInChildren<StatusIcon>())
            iconPool.ReturnObject(icon);

        // 状態の数だけ再表示
        for (int i = 0; i < _self.Effects.Count; i++)
        {
            StatusEffect e = _self.Effects[i];
            int rowIndex = i / 8;
            if (rowIndex >= rows.Length) break;

            StatusIcon icon = iconPool.GetObject();
            icon.transform.SetParent(rows[rowIndex]);
            icon.transform.localScale = Vector3.one;
            icon.SetStatus(e);
        }
    }
}