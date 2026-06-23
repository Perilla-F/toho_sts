using Cysharp.Threading.Tasks;

public static class EnemyEffectResolver
{
    public static void ResolveEffect(IBattleUnit self, IBattleContext context, EnemyEffect effect, IBattleUnit target)
    {
        // 振り分け不要。ポリモーフィズムが自動で解決してくれる
        effect.Data.Apply(self, context, target, effect.Amount);
    }
}