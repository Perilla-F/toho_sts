using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class CardEffectDefinition : ScriptableObject
{
    public CardEffectType EffectType;

    [TextArea(2, 4)]
    [Tooltip("例: 敵に {0} ダメージを与える")]
    public string DescriptionTemplate;

    public abstract void Execute(ICardContext context, IBattleUnit target, int amount);

    public virtual string GetPreviewDescription(int baseAmount, IReadOnlyBattleContext playerContext, IReadOnlyEnemyUnit target = null)
    {
        int previewAmount = baseAmount;

        if (playerContext != null)
        {
            if (EffectType == CardEffectType.Damage)
            {
                if (target == null)
                {
                    previewAmount += playerContext.Hero.StatusCount("strength");
                    if (playerContext.Hero.HasStatus("fear"))
                    {
                        previewAmount = Mathf.FloorToInt(previewAmount * 0.75f);
                    }
                }
                else
                {
                    previewAmount = DamageCalculator.CalculateDamage(playerContext.Hero, target, baseAmount);
                }
            }
            else if (EffectType == CardEffectType.ApplyBlock || EffectType == CardEffectType.SimpleBlock)
            {
                previewAmount += playerContext.Hero.Effects.FirstOrDefault(e => e.Data.EffectId == "defence")?.Stacks ?? 0;
            }
        }

        return String.Format(DescriptionTemplate, previewAmount.ToString());
    }
}