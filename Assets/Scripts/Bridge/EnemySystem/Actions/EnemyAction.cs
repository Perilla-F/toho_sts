using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAction
{
    public string actionName;
    public Sprite icon;
    public IBattleUnit Self;
    public EnemyActionTarget target;
    public int ScheduledTime;
    public List<EnemyEffectData> Effects; // 複数効果
    public bool IsCanceled;

    public void Perform(IBattleContext context)
    {
        if (IsCanceled) return;
        foreach (var effect in Effects)
        {
            effect.Apply(context, Self);
        }
    }
}