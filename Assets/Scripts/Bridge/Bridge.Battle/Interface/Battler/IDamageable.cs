public interface IDamageable : IReadOnlyDamageable
{
    public void UpdateHpBar();
}

public interface IReadOnlyDamageable
{
    int CurrentHP { get; }
    public bool HasStatus(string effectId);
    public int StatusCount(string effectId);
}