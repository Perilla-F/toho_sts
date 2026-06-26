using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HPResource : IResource
{
    public ResourceType Type => ResourceType.HP;
    public int MaxHP { get; private set; }
    public int CurrentResource { get; private set; }

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

    public bool IsAlive()
    {
        return CurrentResource > 0;
    }

    public void TakeDamage(int amount)
    {
        CurrentResource = Mathf.Max(0, CurrentResource - amount);
    }

    public bool TryConsume(int cost)
    {
        if (CurrentResource < cost) return false;

        CurrentResource -= cost;
        return true;
    }

    public void Gain(int amount)
    {
        CurrentResource = Mathf.Min(MaxHP, CurrentResource + amount);
    }

    public void LoseHP(int amount)
    {
        CurrentResource = Mathf.Max(0, CurrentResource - amount);
    }

}
