using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartRest();
    }
}
