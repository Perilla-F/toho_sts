public class PoisonEffect : IEffect
{
    public string Name => "毒";
    public string Description => "ターン終了時にスタック分のダメージを受ける";
    private int _amount;
    public bool IsDebuff => true;
    public bool IsExpired => _amount <= 0;

    public PoisonEffect(int amount)
    {
        this._amount = amount;
    }

    public void OnApply(IBattleUnit target)
    {
    }
    public void OnTurnStart(IBattleUnit target) { }
    public void OnTurnEnd(IBattleUnit target)
    {
        target.TakeDamage(_amount);
        _amount--;
    }
    public void OnRemove(IBattleUnit target) { }
}
