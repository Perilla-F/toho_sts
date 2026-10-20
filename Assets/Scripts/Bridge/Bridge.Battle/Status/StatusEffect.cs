using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StatusEffect : IReadOnlyStatusEffect
{
    public EffectData Data { get; protected set; }
    public int Stacks { get; protected set; }
    protected IBattleUnit _owner;
    public IBattleUnit Owner => _owner;
    IReadOnlyBattleUnit IReadOnlyStatusEffect.Owner => _owner;

    public StatusEffect(EffectData data, int initialStacks, IBattleUnit owner)
    {
        Data = data;
        Stacks = initialStacks;
        _owner = owner;
    }

    public virtual void AddStacks(int amount)
    {
        Stacks += amount;
        BattleEventBus.View.OnUpdateBuffIcon(_owner, this);

    }

    public virtual void RemoveStacks(int amount)
    {
        Stacks = Mathf.Max(0, Stacks - amount);
        BattleEventBus.View.OnUpdateBuffIcon(_owner, this);
    }

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
}

public interface IReadOnlyStatusEffect
{
    public EffectData Data { get; }
    public int Stacks { get; }
    public IReadOnlyBattleUnit Owner { get; }
}