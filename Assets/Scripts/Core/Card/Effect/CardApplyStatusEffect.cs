using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Status")]
public class CardApplyStatusEffect : CardEffectDefinition
{
    private StatusEffectData _data;

    public override async UniTask Apply(int amount, CardContext context)
    {
        foreach (var target in context.Targets)
        {
            target.AddEffect(_data, amount);
        }
    }
}