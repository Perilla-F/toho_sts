using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HeroData selectedHeroData;
    public HeroBattler heroBattler;
    public List<SourceCard> playerDeck = new List<SourceCard>();
    public int currentHP;

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
        heroBattler = new HeroBattler(selectedHeroData, selectedHeroData.MaxHP);
        for (int i = 0; i < selectedHeroData.startingDeck.Count; i++)
        {
            AddCard(new SourceCard(selectedHeroData.startingDeck[i]));
        }
    }

    public List<SourceCard> GetPlayerDeck()
    {
        return playerDeck;
    }

    public void AddCard(SourceCard card)
    {
        playerDeck.Add(card);
    }

    public void RemoveCard(SourceCard card)
    {
        playerDeck.Remove(card);
    }

    public void UpdateAfterBattle(HeroBattler battler, List<SourceCard> updatedDeck)
    {
        currentHP = battler.CurrentHP;
        playerDeck = new List<SourceCard>(updatedDeck);
    }
}
