using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class Mana : IResource
{
    public ResourceType Type => ResourceType.Mana;
    public int MaxMana { get; private set; }
    public int CurrentResource { get; private set; }
    public event Action OnChanged;

    public Mana(int maxMana)
    {
        this.MaxMana = maxMana;
        CurrentResource = maxMana;
    }

    public int GetMana()
    {
        return CurrentResource;
    }

    public bool TryConsume(int cost)
    {
        if (CurrentResource < cost) return false;

        CurrentResource -= cost;
        OnChanged?.Invoke();
        return true;
    }

    public async UniTask Gain(int amount)
    {
        CurrentResource += amount;
        OnChanged?.Invoke();
    }

    public void RefleshMana()
    {
        CurrentResource = MaxMana;
        OnChanged?.Invoke();
    }
}
