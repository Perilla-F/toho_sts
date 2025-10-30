using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnEndButton : MonoBehaviour
{
    public event Action OnClickTurnEnd;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnClickTurnEnd?.Invoke());
    }
}
