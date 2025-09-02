public class DrawEffectExecutor : ICardEffectExecutor
{
    public void Execute(CardEffectData data, CardContext context)
    {
        context.BattleSystem.Draw(data.Value);
    }
}
