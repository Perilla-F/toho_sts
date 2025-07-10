using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }
}