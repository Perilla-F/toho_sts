using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartElite();
    }
}
