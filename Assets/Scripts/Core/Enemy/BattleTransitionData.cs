using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ→戦闘遷移時に渡すデータ
/// </summary>
[Serializable]
public class BattleTransitionData
{
    /// <summary>プレイヤーの状態</summary>
    public HeroBattler Hero;
    public EncounterData EncounterData;

    public BattleTransitionData(HeroBattler hero, EncounterData data)
    {
        Hero = hero;
        EncounterData = data;
    }
}
