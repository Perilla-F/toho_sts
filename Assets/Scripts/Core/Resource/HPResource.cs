using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HPResource : IResource
{
    public ResourceType Type => ResourceType.HP;
    public int MaxHP { get; private set; }
    public int CurrentResource { get; private set; }
    public event Action OnChanged;

    public int Block;
    public int SimpleBlock;

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
        OnChanged?.Invoke();
    }

    public bool IsAlive()
    {
        return CurrentResource > 0;
    }

    public async UniTask TakeDamage(int amount)
    {
        int remainedDamage = 0;
        remainedDamage = ReceiveSimpleBlock(amount);
        remainedDamage = ReceiveBlock(remainedDamage);
        CurrentResource = Mathf.Max(0, CurrentResource - remainedDamage);
        OnChanged?.Invoke();
    }

    public int ReceiveSimpleBlock(int amount)
    {
        int remainedDamage = 0;
        if (SimpleBlock > amount)
        {
            SimpleBlock -= amount;
        }
        else
        {
            remainedDamage = amount - SimpleBlock;
            SimpleBlock = 0;
        }
        OnChanged?.Invoke();
        return remainedDamage;
    }

    public int ReceiveBlock(int amount)
    {
        int remainedDamage = 0;
        if (Block > amount)
        {
            Block -= amount;
        }
        else
        {
            remainedDamage = amount - Block;
            Block = 0;
        }
        OnChanged?.Invoke();
        return remainedDamage;
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
        Mathf.Min(MaxHP, CurrentResource + amount);
        OnChanged?.Invoke();
    }

    public async UniTask LoseHP(int amount)
    {
        CurrentResource = Mathf.Max(0, CurrentResource - amount);
        OnChanged?.Invoke();
    }

    public async UniTask ApplySimpleBlock(int amount)
    {
        SimpleBlock += amount;
        OnChanged?.Invoke();
    }

    public async UniTask ApplyBlock(int amount)
    {
        Block += amount;
        OnChanged?.Invoke();
    }

    public void ClearSimpleBlock()
    {
        SimpleBlock = 0;
        OnChanged?.Invoke();
    }

    public void ClearBlock()
    {
        Block = 0;
        OnChanged?.Invoke();
    }

}
