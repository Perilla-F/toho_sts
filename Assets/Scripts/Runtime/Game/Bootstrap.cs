using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private string nextScene = "TitleScene";

    private void Start()
    {
        // Managerの初期化（存在しなければ自動生成される）
        GameManager.Instance.InitializeGame();
        SaveManager.Instance.LoadGame();
        GameCoordinator.Instance.InitializeGame();

        // 次のシーンを非同期でロード
        SceneManager.LoadSceneAsync(nextScene);
    }
}
