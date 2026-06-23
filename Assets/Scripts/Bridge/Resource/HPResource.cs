using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HPResource : IResource
{
    public ResourceType Type => ResourceType.HP;
    public int MaxHP { get; private set; }
    public int CurrentResource { get; private set; }

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
        BattleEventBus.View.OnChangedHPCount();
    }

    public bool IsAlive()
    {
        return CurrentResource > 0;
    }

    public void TakeDamage(int amount)
    {
        int remainedDamage = 0;
        remainedDamage = ReceiveSimpleBlock(amount);
        remainedDamage = ReceiveBlock(remainedDamage);
        CurrentResource = Mathf.Max(0, CurrentResource - remainedDamage);
        BattleEventBus.View.OnChangedHPCount();
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
        BattleEventBus.View.OnChangedHPCount();
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
        BattleEventBus.View.OnChangedHPCount();
        return remainedDamage;
    }

    public bool TryConsume(int cost)
    {
        if (CurrentResource < cost) return false;

        CurrentResource -= cost;
        BattleEventBus.View.OnChangedHPCount();
        return true;
    }

    public void Gain(int amount)
    {
        CurrentResource = Mathf.Min(MaxHP, CurrentResource + amount);
        BattleEventBus.View.OnChangedHPCount();
    }

    public void LoseHP(int amount)
    {
        CurrentResource = Mathf.Max(0, CurrentResource - amount);
        BattleEventBus.View.OnChangedHPCount();
    }

    public void ApplySimpleBlock(int amount)
    {
        SimpleBlock += amount;
        BattleEventBus.View.OnChangedHPCount();
    }

    public void ApplyBlock(int amount)
    {
        Block += amount;
        BattleEventBus.View.OnChangedHPCount();
    }

    public void ClearSimpleBlock()
    {
        SimpleBlock = 0;
        BattleEventBus.View.OnChangedHPCount();
    }

    public void ClearBlock()
    {
        Block = 0;
        BattleEventBus.View.OnChangedHPCount();
    }

}
