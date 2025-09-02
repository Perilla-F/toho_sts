using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearEffect : IEffect
{
    public string Name => "恐怖";
    public string Description => "攻撃時に最終ダメージが0.75倍になる(切り上げ)";
    private int _amount;
    public bool IsDebuff => true;
    public bool IsExpired => _amount <= 0;

    public FearEffect(int amount)
    {
        this._amount = amount;
    }

    public void OnApply(IBattleUnit target) { /* 重複時スタック加算 */ }
    public void OnTurnStart(IBattleUnit target) { }
    public void OnTurnEnd(IBattleUnit target)
    {
        _amount--;
    }
    public void OnRemove(IBattleUnit target) { }
}
