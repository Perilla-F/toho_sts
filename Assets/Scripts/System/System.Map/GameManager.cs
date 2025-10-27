using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : IGameManager
{
    public GameContext Context { get; private set; }

    private readonly ISceneLoader _sceneLoader;

    public GameManager(ISceneLoader sceneLoader)
    {
        Context = new GameContext();
        _sceneLoader = sceneLoader;
    }

    public void InitializeGame()
    {
        Debug.Log("GameManager initialized!");
    }

    public void InitializePlayer(HeroData heroData)
    {
        var selectedHeroData = heroData;
        HPResource hPResource = new HPResource(heroData.MaxHP);
        Mana mana = new Mana(heroData.MaxMana);
        var heroBattler = new HeroBattler(heroData, hPResource, mana);
        var playerDeck = new List<SourceCard>();

        for (int i = 0; i < heroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(heroData.StartingDeck[i], hPResource, mana));
        }

        Context.SelectedHeroData = selectedHeroData;
        Context.HeroBattler = heroBattler;
        Context.PlayerDeck = playerDeck;
    }

    public void AddCard(SourceCard card)
    {
        Context.PlayerDeck.Add(card);
    }

    public void RemoveCard(SourceCard card)
    {
        Context.PlayerDeck.Remove(card);
    }

    public void UpdateDeckAfterBattle(List<SourceCard> updatedDeck)
    {
        Context.PlayerDeck = new List<SourceCard>(updatedDeck);
    }

    public void StartBattle(EnemyType type)
    {
        var encounters = EncounterLoader.LoadEncounters(type, Context.StageIndex);
        var selected = encounters[UnityEngine.Random.Range(0, encounters.Count)];
        var data = new BattleTransitionData(Context.HeroBattler, selected);
        _sceneLoader.SetTransitionData(data);
        _sceneLoader.LoadScene("BattleScene");
    }

    public void SaveMap(MapSaveData data)
    {
        Context.MapSaveData = data;
    }

    public void SaveEvent(EventSaveData data)
    {
        Context.EventSaveData = data;
    }

}
