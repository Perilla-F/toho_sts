public interface IMana : IReadOnlyMana
{
    public void RefleshMana();
}

public interface IReadOnlyMana
{
    public ResourceType Type { get; }
    public int MaxMana { get; }
    public int CurrentResource { get; }
}