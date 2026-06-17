using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class EnemyUI : MonoBehaviour, IBattleUI
{
    [Header("UI References")]
    [SerializeField] private HpBar hpBar;
    [SerializeField] private Transform actionContainer;
    [SerializeField] private Transform buffContainer;

    [SerializeField] private GameObject actionIconPrefab;
    [SerializeField] private GameObject buffIconPrefab;

    private Vector3 _baseScale;
    private List<ActionIcon> actionList;
    private List<BuffIcon> buffList;

    private void Start()
    {
    }

    public void Bind(HPResource resource)
    {
        hpBar.Bind(resource);
    }

    public void SetActionIcon(EnemyActionType type, int time, int number)
    {
        ActionIcon actionIcon = Instantiate(actionIconPrefab, actionContainer).GetComponent<ActionIcon>();
        actionIcon.SetIcon(type, time, number);
        actionList.Add(actionIcon);
    }

    public void SetBuffIcon(StatusEffect data)
    {
        BuffIcon buffIcon = Instantiate(buffIconPrefab, buffContainer).GetComponent<BuffIcon>();
        buffIcon.SetIcon(data);
        buffList.Add(buffIcon);
    }

    public void UpdateBuffIcon(StatusEffect data)
    {
        var buffIcon = buffList.FirstOrDefault(l => l.effect.Data.effectId == data.Data.effectId);
        buffIcon.UpdateIcon(data);
    }

    public void Highlight(bool active, int number)
    {
        if (actionList.Count() > 0)
        {
            var actionIcon = actionList.FirstOrDefault(l => l.number == number);
            actionIcon.HighlightIcon(active);
        }
    }

    public void ShowDamageEffect(float duration)
    {
        // 被ダメージエフェクト処理など
    }

}
