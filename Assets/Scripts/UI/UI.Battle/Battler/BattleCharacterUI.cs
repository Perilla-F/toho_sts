using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BattleCharacterUI : MonoBehaviour, IBattleUI
{
    [Header("UI References")]
    [SerializeField] protected HpBar hpBar;
    [SerializeField] protected Transform buffContainer;

    [SerializeField] protected GameObject buffIconPrefab;

    protected List<BuffIcon> buffList = new List<BuffIcon>();

    public virtual void Bind(HPResource resource)
    {
        hpBar.Bind(resource);
    }

    public virtual void SetBuffIcon(StatusEffect data)
    {
        BuffIcon buffIcon = Instantiate(buffIconPrefab, buffContainer).GetComponent<BuffIcon>();
        buffIcon.SetIcon(data);
        buffList.Add(buffIcon);
    }

    public virtual void UpdateBuffIcon(StatusEffect data)
    {
        var buffIcon = buffList.FirstOrDefault(l => l.effect.Data.EffectId == data.Data.EffectId);
        buffIcon.UpdateIcon(data);
    }

}