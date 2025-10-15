using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => player.EndTurnButton());
    }
}