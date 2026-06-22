using Cysharp.Threading.Tasks;

public static class EffectResolver
{
    public static void ResolveEffect(EnemyEffect effect, IBattleUnit target)
    {
        switch (effect.Data.EffectId)
        {
            case "damage":
                target.TakeDamageAsync(effect.Amount);
                break;
            case "block":
                target.ApplyBlock(effect.Amount);
                break;
            case "simpleBlock":
                target.ApplySimpleBlock(effect.Amount);
                break;
            default:
                target.AddEffect(effect.Data, effect.Amount);
                break;
        }
    }
}