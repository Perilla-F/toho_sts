using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler;
    public List<SourceCard> PlayerDeck = new List<SourceCard>();
    public int CurrentHP;

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
