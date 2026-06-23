using System.Collections.Generic;

public static class CardEffectResolver
{
    public static void ResolveEffect(ITargetSelector selector, CardEffectDefinition effectLogic, int amount, ICardContext context)
    {
        var targets = selector.GetTargets(context);
        foreach (var target in targets)
        {
            effectLogic.Execute(context, target, amount);
        }
    }
}