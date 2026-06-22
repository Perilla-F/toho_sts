using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroBattler
{
    public HeroData BaseData { get; private set; }
    public HPResource HPResource;
    public Mana Mana;
    public int DrawCount { get; private set; } = 5;

    public HeroBattler(HeroData data, HPResource hPResource, Mana mana)
    {
        BaseData = data;
        HPResource = hPResource;
        Mana = mana;
        DrawCount = data.DrawCount;
    }

}