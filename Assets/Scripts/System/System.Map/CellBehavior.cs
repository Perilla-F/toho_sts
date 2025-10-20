using UnityEngine;

public abstract class CellBehavior : MonoBehaviour
{
    public System.Action<EncounterData> OnBattleRequest;

    public abstract void OnPlayerEnter();
}