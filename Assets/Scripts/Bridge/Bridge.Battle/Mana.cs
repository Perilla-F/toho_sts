using System;
using System.Collections;
using System.Collections.Generic;

public class Mana : IResource
{
    public ResourceType Type => ResourceType.Mana;
    public int MaxMana { get; private set; }
    public int CurrentMana { get; private set; }
    public event Action OnChanged;

    public Mana(int maxMana)
    {
        this.MaxMana = maxMana;
        CurrentMana = maxMana;
    }

    public int GetMana()
    {
        return CurrentMana;
    }

    public bool TryConsume(int cost)
    {
        if (CurrentMana < cost) return false;

        CurrentMana -= cost;
        OnChanged?.Invoke();
        return true;
    }

    public void Gain(int amount)
    {
        CurrentMana += amount;
        OnChanged?.Invoke();
    }

    public void RefleshMana()
    {
        CurrentMana = MaxMana;
        OnChanged?.Invoke();
    }
}
