using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCell : CellBehaviour
{
    public override void OnPlayerEnter()
    {
        MapGenerator.Instance.StartShop();
    }
}
