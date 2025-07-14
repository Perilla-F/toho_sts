using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartBattle();
    }
}
