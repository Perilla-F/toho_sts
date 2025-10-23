using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroBattler
{
    private HeroData _baseData;
    public HeroData BaseData { get => _baseData; }
    public HPResource HPResource;
    public Mana Mana;
    public int DrawCount { get; private set; } = 5;

    public HeroBattler(HeroData data, HPResource hPResource, Mana mana)
    {
        _baseData = data;
        HPResource = hPResource;
        Mana = mana;
        DrawCount = data.DrawCount;
    }

}