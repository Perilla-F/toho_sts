using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartTreasure();
    }
}
