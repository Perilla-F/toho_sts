using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private GameContext context;

    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck = new List<SourceCard>();
    public EncounterData CurrentEncounter;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        context = new GameContext(this);
    }

    public GameContext GetGameContext()
    {
        return context;
    }

    public void InitializeGame()
    {
        Debug.Log("GameManager initialized!");
    }

    public void InitializePlayer(HeroData heroData)
    {
        SelectedHeroData = heroData;
        HPResource hPResource = new HPResource(heroData.MaxHP);
        Mana mana = new Mana(heroData.MaxMana);
        HeroBattler = new HeroBattler(heroData, hPResource, mana);
        for (int i = 0; i < heroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(heroData.StartingDeck[i], hPResource, mana));
        }
    }

    public HeroData GetSelectedHeroData()
    {
        return SelectedHeroData;
    }

    public HeroBattler GetHeroBattler()
    {
        return HeroBattler;
    }

    public List<SourceCard> GetPlayerDeck()
    {
        return PlayerDeck;
    }

    public EncounterData GetEncounterData()
    {
        return CurrentEncounter;
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

    public PlayerSaveData CreateSaveData()
    {
        return new PlayerSaveData(
            HeroBattler,
            PlayerDeck
        );
    }

    public void LoadFromData(PlayerSaveData data)
    {
        HeroBattler = data.HeroBattler;
        PlayerDeck = data.PlayerDeck;
    }
}
