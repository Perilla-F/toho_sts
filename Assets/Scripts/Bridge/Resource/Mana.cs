using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cysharp.Threading.Tasks;

public class Mana : IResource, IMana
{
    public ResourceType Type => ResourceType.Mana;
    public int MaxMana { get; private set; }
    public int CurrentResource { get; private set; }

    public Mana(int maxMana)
    {
        this.MaxMana = maxMana;
        CurrentResource = maxMana;
    }

    public bool TryConsume(int cost)
    {
        if (CurrentResource < cost) return false;
        UnityEngine.Debug.Log($"Consume {cost} Mana!");

        CurrentResource -= cost;
        BattleEventBus.View.OnChangedManaCount?.Invoke();
        return true;
    }

    public void Gain(int amount)
    {
        CurrentResource += amount;
        BattleEventBus.View.OnChangedManaCount?.Invoke();
    }

    public void RefleshMana()
    {
        CurrentResource = MaxMana;
        BattleEventBus.View.OnChangedManaCount?.Invoke();
    }
}
