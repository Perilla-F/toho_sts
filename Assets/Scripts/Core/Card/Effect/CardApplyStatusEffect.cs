using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Status")]
public class CardApplyStatusEffect : CardEffectDefinition
{
    private EffectData _data;

    public override async UniTask Apply(int amount, CardContext context)
    {
        foreach (var target in context.Targets)
        {
            await target.AddEffect(_data, amount);
        }
    }
}