using System;
using System.Collections;
using System.Collections.Generic;

public class HPResource : IResource
{
    public ResourceType Type => ResourceType.Mana;
    public int MaxHP { get; private set; }
    public int CurrentResource { get; private set; }
    public event Action OnChanged;

    public HPResource(int maxHP)
    {
        MaxHP = maxHP;
        CurrentResource = maxHP;
    }

    public int GetHP()
    {
        return CurrentResource;
    }

    public void SetHP(int amount)
    {
        CurrentResource = amount;
    }

    public bool TryConsume(int cost)
    {
        if (CurrentResource < cost) return false;

        CurrentResource -= cost;
        OnChanged?.Invoke();
        return true;
    }

    public void Gain(int amount)
    {
        CurrentResource += amount;
        OnChanged?.Invoke();
    }

}
