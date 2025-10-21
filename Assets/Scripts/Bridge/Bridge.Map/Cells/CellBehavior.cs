using UnityEngine;

public abstract class CellBehavior
{
    public System.Action<EncounterData> OnBattleRequest;
    public MultiStepEvent AssignedEvent;

    public abstract void OnPlayerEnter();
}