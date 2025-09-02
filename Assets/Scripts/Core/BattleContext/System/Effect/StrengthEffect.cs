using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthEffect : IEffect
{
    public string Name => "攻撃補正";
    public string Description => "ダメージにスタック分の補正がかかる";
    private int _amount;
    public bool IsDebuff => false;
    public bool IsExpired => _amount == 0;

    public StrengthEffect(int amount)
    {
        this._amount = amount;
    }

    public void OnApply(IBattleUnit target)
    {
        target.Strength = _amount;
    }
    public void OnTurnStart(IBattleUnit target) { }
    public void OnTurnEnd(IBattleUnit target) { }
    public void OnRemove(IBattleUnit target) { }
}
