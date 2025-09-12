using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck = new List<SourceCard>();
    public int CurrentHP;
    public EnemyDatabase EnemyDB { get; private set; }


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

    private void LoadMasterData()
    {
        string enemyJson = LoadJsonFromResources("Data/enemies");
        string effectJson = LoadJsonFromResources("Data/effects");
    }

    private string LoadJsonFromResources(string path)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);
        return jsonFile != null ? jsonFile.text : throw new Exception($"JSON not found: {path}");
    }

    public void InitializePlayer()
    {
        HeroBattler = new HeroBattler(SelectedHeroData, SelectedHeroData.MaxHP);
        for (int i = 0; i < SelectedHeroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(SelectedHeroData.StartingDeck[i]));
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

    public void UpdateAfterBattle(HeroBattler battler, List<SourceCard> updatedDeck)
    {
        CurrentHP = battler.CurrentHP;
        PlayerDeck = new List<SourceCard>(updatedDeck);
    }
}
