public class BlockEffectExecutor : ICardEffectExecutor
{
    public void Execute(CardEffectData data, CardContext context)
    {
        context.User.ApplyBlock(data.Value);
    }
}
