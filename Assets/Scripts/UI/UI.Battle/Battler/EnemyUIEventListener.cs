using System.Collections.Generic;
using UnityEngine;

public class EnemyUIEventListener
{
    private int _enemyId;
    private EnemyUI _ui;

    public void Initialize(int enemyId, EnemyUI ui)
    {
        _enemyId = enemyId;
        _ui = ui;

        BuffUIChannel.OnBuffAdded += HandleBuffAdd;
        BuffUIChannel.OnBuffUpdated += HandleBuffUpdate;
    }

    private void HandleBuffAdd(StatusEffect data)
    {
        _ui.SetBuffIcon(data);
    }

    private void HandleBuffUpdate(StatusEffect data)
    {
        _ui.UpdateBuffIcon(data);
    }

}
