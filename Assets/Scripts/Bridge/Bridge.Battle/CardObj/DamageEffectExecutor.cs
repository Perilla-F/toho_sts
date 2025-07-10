public class DamageEffectExecutor : ICardEffectExecutor
{
    public void Execute(CardEffectData data, CardContext context)
    {
        foreach (var target in context.Targets)
            target.TakeDamage(data.value);
    }
}
