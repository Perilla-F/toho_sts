using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroBattler
{
    public HeroData BaseData { get; private set; }
    public HPResource HPResource { get; private set; }
    public Mana Mana { get; private set; }
    public int DrawCount { get; private set; }

    public HeroBattler(HeroData data, HPResource hPResource, Mana mana)
    {
        BaseData = data;
        HPResource = hPResource;
        Mana = mana;
        DrawCount = data.DrawCount;
    }

}