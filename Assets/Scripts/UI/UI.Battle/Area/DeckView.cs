using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckView : MonoBehaviour, IDeckView
{
    [SerializeField] private TextMeshProUGUI _deckCountText;

    public void UpdateDeckCount(int count)
    {
        _deckCountText.text = count.ToString();
    }
}