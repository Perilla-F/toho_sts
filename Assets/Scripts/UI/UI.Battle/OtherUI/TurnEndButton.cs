using UnityEngine;
using UnityEngine.UI;

public class TurnEndButton : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => player.TurnEndButton());
    }
}
