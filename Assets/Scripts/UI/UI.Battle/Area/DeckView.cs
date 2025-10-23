using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckView : MonoBehaviour, IDeckView
{
    [SerializeField] private TextMeshProUGUI _deckCountText;
    private BattleDeck _battleDeck;
    public void UpdateDeckCount()
    {
        _deckCountText.text = _battleDeck.Count.ToString();
    }

    public void SetBattleDeck(BattleDeck battleDeck)
    {
        this._battleDeck = battleDeck;
    }
}