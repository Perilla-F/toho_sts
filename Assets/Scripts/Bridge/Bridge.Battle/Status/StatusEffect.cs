using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StatusEffect
{
    public EffectData Data { get; protected set; }
    public int Stacks { get; protected set; }
    public IBattleUnit Owner { get; protected set; }

    public StatusEffect(EffectData data, int initialStacks, IBattleUnit owner)
    {
        Data = data;
        Stacks = initialStacks;
        Owner = owner;
    }

    public virtual void AddStacks(int amount)
    {
        Stacks += amount;
        BattleEventBus.View.OnUpdateBuffIcon(Owner, this);

    }

    public virtual void RemoveStacks(int amount)
    {
        Stacks = Mathf.Max(0, Stacks - amount);
        BattleEventBus.View.OnUpdateBuffIcon(Owner, this);
    }

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
}
