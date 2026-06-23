using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class StatusEffect
{
    public EffectData Data { get; private set; }
    public int Stacks { get; private set; }
    protected IBattleUnit Owner { get; private set; }

    protected StatusEffect(EffectData data, int initialStacks, IBattleUnit owner)
    {
        Data = data;
        Stacks = initialStacks;
        Owner = owner;
    }

    public void AddStacks(int amount) => Stacks += amount;
    public void RemoveStacks(int amount) => Stacks = Mathf.Max(0, Stacks - amount);

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
}
