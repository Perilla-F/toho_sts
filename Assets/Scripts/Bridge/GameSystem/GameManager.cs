using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var prefab = Resources.Load<GameManager>("Prefabs/Managers/GameManager");
                if (prefab != null)
                {
                    Instantiate(prefab);
                }
                else
                {
                    Debug.LogError("GameManager prefab not found in Resources!");
                }
            }
            return instance;
        }
    }

    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck = new List<SourceCard>();
    public EncounterData CurrentEncounter;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeGame()
    {
        Debug.Log("GameManager initialized!");
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
