using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    private void Start()
    {
        foreach (var cell in FindObjectsOfType<BattleCell>())
        {
            // DI: Battle開始リクエストが来たらScene遷移
            cell.OnBattleRequest = encounter => StartCoroutine(StartBattleScene(encounter));
        }
    }

    private IEnumerator StartBattleScene(EncounterData encounter)
    {
        // Encounterを保存（方法A: GameManager経由）
        GameManager.Instance.CurrentEncounter = encounter;

        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BattleScene");
        while (!op.isDone) yield return null;

        // 遷移後に BattleStarter が Awake して Encounterを読む
    }
}