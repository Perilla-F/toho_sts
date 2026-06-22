using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HeroUI : BattleCharacterUI
{

    private IHeroUnit Hero;

    public void Init(IHeroUnit hero)
    {
        Hero = hero;
    }

}