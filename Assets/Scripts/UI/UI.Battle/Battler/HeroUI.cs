using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HeroUI : MonoBehaviour, IBattleUI
{
    [SerializeField] private HpBar hpBar;
    [SerializeField] private Transform buffContainer;

    [SerializeField] private GameObject buffIconPrefab;

    private IHeroUnit Hero;
    private List<BuffIcon> buffList;

    public void Init(IHeroUnit hero)
    {
        Hero = hero;
    }

    public void Bind(HPResource resource)
    {
        hpBar.Bind(resource);
    }

    public void SetBuffIcon(StatusEffect data)
    {
        BuffIcon buffIcon = Instantiate(buffIconPrefab, buffContainer).GetComponent<BuffIcon>();
        buffIcon.SetIcon(data);
        buffList.Add(buffIcon);
    }

    public void UpdateBuffIcon(StatusEffect data)
    {
        var buffIcon = buffList.FirstOrDefault(l => l.effect.Data.EffectId == data.Data.EffectId);
        buffIcon.UpdateIcon(data);
    }

}