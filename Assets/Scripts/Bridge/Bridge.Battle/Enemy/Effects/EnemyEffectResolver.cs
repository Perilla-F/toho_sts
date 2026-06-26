using Cysharp.Threading.Tasks;

public static class EnemyEffectResolver
{
    public static void ResolveEffect(int id, IBattleContext context, EnemyEffect effect, IBattleUnit target)
    {
        // 振り分け不要。ポリモーフィズムが自動で解決してくれる
        effect.Data.Apply(id, context, target, effect.Amount);
    }
}