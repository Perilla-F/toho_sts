using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck = new List<SourceCard>();
    public EncounterData CurrentEncounter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePlayer()
    {
        HPResource hPResource = new HPResource(SelectedHeroData.MaxHP);
        Mana mana = new Mana(SelectedHeroData.MaxMana);
        HeroBattler = new HeroBattler(SelectedHeroData, hPResource, mana);
        for (int i = 0; i < SelectedHeroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(SelectedHeroData.StartingDeck[i], hPResource, mana));
        }
    }

    public List<SourceCard> GetPlayerDeck()
    {
        return PlayerDeck;
    }

    public void AddCard(SourceCard card)
    {
        PlayerDeck.Add(card);
    }

    public void RemoveCard(SourceCard card)
    {
        PlayerDeck.Remove(card);
    }

    public void UpdateAfterBattle(List<SourceCard> updatedDeck)
    {
        PlayerDeck = new List<SourceCard>(updatedDeck);
    }
}
