using Cysharp.Threading.Tasks;

public static class EffectResolver
{
    public static async UniTask ResolveEffect(EnemyEffect effect, BattleUnit target)
    {
        switch (effect.Data.EffectId)
        {
            case "damage":
                await target.TakeDamageAsync(effect.Amount);
                break;
            case "block":
                await target.ApplyBlock(effect.Amount);
                break;
            default:
                await target.AddEffect(effect.Data, effect.Amount);
                break;
        }
    }
}