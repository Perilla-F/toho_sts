using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckView : MonoBehaviour, IDeckView
{
    [SerializeField] private TextMeshProUGUI deckCountText;
    BattleDeck battleDeck;
    public void UpdateDeckCount()
    {
        deckCountText.text = battleDeck.Count.ToString();
    }

    public void SetBattleDeck(BattleDeck battleDeck)
    {
        this.battleDeck = battleDeck;
    }
}