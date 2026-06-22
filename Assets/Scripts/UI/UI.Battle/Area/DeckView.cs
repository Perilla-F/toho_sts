using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deckCountText;

    private IReadOnlyBattleDeck _deck;

    public void Setup(IReadOnlyBattleDeck deck)
    {
        _deck = deck;
        UpdateDeckCount();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void UpdateDeckCount()
    {
        _deckCountText.text = _deck.Count.ToString();
    }
}