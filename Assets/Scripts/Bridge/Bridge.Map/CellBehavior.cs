using UnityEngine;

public abstract class CellBehavior : MonoBehaviour
{
    // DI用のイベント（MapManager から注入される）
    public System.Action<EncounterData> OnBattleRequest;

    public abstract void OnPlayerEnter();
}