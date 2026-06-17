using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StatusEffect
{
    public StatusEffectData Data { get; private set; }
    public int Stacks { get; private set; }
    protected BattleUnit Owner { get; private set; }

    protected StatusEffect(StatusEffectData data, int initialStacks, BattleUnit owner)
    {
        Data = data;
        Stacks = initialStacks;
        Owner = owner;
    }

    public void AddStacks(int amount) => Stacks += amount;
    public void RemoveStacks(int amount) => Stacks = Mathf.Max(0, Stacks - amount);

    public abstract UniTask OnTurnStart();
    public abstract UniTask OnTurnEnd();
}
