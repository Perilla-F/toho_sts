using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class HeroUnit : BaseBattleUnit, IHeroUnit
{
    public Sprite PlayerEventIcon { get; private set; }
    private Mana _mana;
    IMana IHeroUnit.Mana => _mana;
    IReadOnlyMana IReadOnlyHeroUnit.Mana => _mana;
    public int DrawCount { get; private set; }


    public void Setup(HeroBattler heroBattler)
    {
        HPResource = heroBattler.HPResource;
        BattlerName = heroBattler.BaseData.BattlerName;
        _mana = heroBattler.Mana;
        DrawCount = heroBattler.DrawCount;

        PlayerEventIcon = heroBattler.BaseData.playerEventIcon;

        IdleClip = heroBattler.BaseData.IdleClip;
        AttackClip = heroBattler.BaseData.AttackClip;
        HitClip = heroBattler.BaseData.HitClip;

        Effects = new List<StatusEffect>();
        DefenseComponent.Block = 0;
        DefenseComponent.SimpleBlock = 0;
    }

    public void TakeDamageAsync(int amount)
    {
        HPResource.TakeDamage(amount);
    }

    public void GainMana(int amount)
    {
        _mana.Gain(amount);
    }

}