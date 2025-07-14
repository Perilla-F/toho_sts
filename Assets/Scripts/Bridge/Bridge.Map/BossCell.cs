using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartBoss();
    }
}
