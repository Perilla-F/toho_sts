public static class EffectFactory
{
    public static IEffect CreateEffect(EffectType type, int amount)
    {
        switch (type)
        {
            case EffectType.Strength:
                return new StrengthEffect(amount);
            case EffectType.Poison:
                return new PoisonEffect(amount);
            case EffectType.Fear:
                return new FearEffect(amount);
            case EffectType.None:
            default:
                return null;
        }
    }
}
