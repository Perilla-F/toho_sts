using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Status")]
public class CardApplyStatusEffect : CardEffectDefinition
{
    private EffectData _data;

    public override void Apply(int amount, CardContext context)
    {
        foreach (var target in context.Targets)
        {
            target.AddEffect(_data, amount);
        }
    }
}