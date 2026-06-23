public interface IDamageable
{
    int CurrentHP { get; }
    public bool HasStatus(string effectId);
    public int StatusCount(string effectId);
}