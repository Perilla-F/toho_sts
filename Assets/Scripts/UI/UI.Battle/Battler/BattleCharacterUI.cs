using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BattleCharacterUI : MonoBehaviour, IBattleUI
{
    [Header("UI References")]
    [SerializeField] protected HpBar hpBar;
    [SerializeField] protected Transform buffContainer;

    [SerializeField] protected GameObject buffIconPrefab;

    private IReadOnlyBattleUnit _boundUnit;
    protected List<BuffIcon> buffList = new List<BuffIcon>();

    public void Awake()
    {
        BattleEventBus.View.OnUpdateHp += HandleHpChanged;
        BattleEventBus.View.OnUpdateBuffIcon += UpdateBuffIcon;
    }

    public virtual void Bind(IReadOnlyBattleUnit unit)
    {
        _boundUnit = unit;
        hpBar.ApplyVisuals(unit);
    }

    private void HandleHpChanged(IReadOnlyBattleUnit unit)
    {
        // 「イベントで流れてきたユニット」と「自分が表示しているユニット」が同じなら更新
        if (unit == _boundUnit)
        {
            hpBar.ApplyVisuals(unit);
        }
    }

    public virtual void SetBuffIcon(IReadOnlyBattleUnit unit, StatusEffect data)
    {
        if (unit == _boundUnit)
        {
            if (data == null) return;
            BuffIcon buffIcon = Instantiate(buffIconPrefab, buffContainer).GetComponent<BuffIcon>();
            buffIcon.SetIcon(data);
            buffList.Add(buffIcon);
        }
    }

    public virtual void UpdateBuffIcon(IReadOnlyBattleUnit unit, StatusEffect data)
    {
        if (unit == _boundUnit)
        {
            var buffIcon = buffList.FirstOrDefault(l => l.effect.Data.EffectId == data.Data.EffectId);
            if (buffIcon == null)
            {
                SetBuffIcon(unit, data);
            }
            else
            {
                buffIcon.UpdateIcon(data);
            }
        }
    }

    public virtual void OnDestroy()
    {
        BattleEventBus.View.OnUpdateHp -= HandleHpChanged;
        BattleEventBus.View.OnUpdateBuffIcon -= UpdateBuffIcon;
    }

}