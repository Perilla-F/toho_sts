using UnityEngine;
using UnityEngine.SceneManagement;

public class MapFlowController : MonoBehaviour
{
    private MapManager _mapManager;
    private EventManager _eventManager;

    private void Awake()
    {
        _mapManager = ServiceLocator.Get<MapManager>();
        _eventManager = ServiceLocator.Get<EventManager>();
    }

    public void HandleCellEnter(CellType cellType)
    {
        // 現在のマップ状態を更新
        _mapManager.MapSave();

        switch (cellType)
        {
            case CellType.Battle:
                SceneManager.LoadScene("BattleScene");
                break;

            case CellType.Event:
                _eventManager.StartEvent();
                SceneManager.LoadScene("EventScene");
                break;

            case CellType.Shop:
                SceneManager.LoadScene("ShopScene");
                break;

            case CellType.Rest:
                SceneManager.LoadScene("RestScene");
                break;

            case CellType.Goal:
                SceneManager.LoadScene("EndingScene");
                break;
        }
    }
}
