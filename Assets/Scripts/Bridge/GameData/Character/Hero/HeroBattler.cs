using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroBattler
{
    private HeroData _baseData;
    public HeroData BaseData { get => _baseData; }
    public float AttackModifier { get; } = 1f;
    public float DefenceModifier { get; } = 1f;
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }
    public int MaxMana { get; private set; }
    public int Attack { get; } = 0;
    public int Defence { get; } = 0;
    public int Block { get; private set; } = 0;
    public int AttackBonus { get; private set; } = 0;
    public int DefenceBonus { get; private set; } = 0;
    public int DrawCount { get; private set; } = 5;
    public List<string> Status = new List<string>();

    public HeroBattler(HeroData data, int currentHP)
    {
        _baseData = data;
        MaxHP = data.MaxHP;
        CurrentHP = currentHP;
    }

}