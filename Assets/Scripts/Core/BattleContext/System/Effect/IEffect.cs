public interface IEffect
{
    public string Name { get; }
    public string Description { get; }
    public bool IsDebuff { get; }
    public void OnApply(IBattleUnit target);
    public void OnTurnStart(IBattleUnit target);
    public void OnTurnEnd(IBattleUnit target);
    public void OnRemove(IBattleUnit target);
    public bool IsExpired { get; }
}
