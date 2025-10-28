using System.Collections.Generic;

public class PlayerManager : IPlayerManager
{
    public HeroData SelectedHeroData;
    public HeroBattler HeroBattler { get; private set; }

    public List<SourceCard> PlayerDeck;

    public List<string> AcquiredRelics;

    private GameManager _gameManager;

    public PlayerManager(GameManager game)
    {
        _gameManager = game;
    }

    public void Inject(GameManager game)
    {
        _gameManager = game;
    }

    public void InitializePlayer(HeroData heroData)
    {
        SelectedHeroData = heroData;
        HPResource hPResource = new HPResource(heroData.MaxHP);
        Mana mana = new Mana(heroData.MaxMana);
        HeroBattler = new HeroBattler(heroData, hPResource, mana);
        PlayerDeck = new List<SourceCard>();

        for (int i = 0; i < heroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(heroData.StartingDeck[i], hPResource, mana));
        }
    }

    public void AddCard(SourceCard card)
    {
        PlayerDeck.Add(card);
    }

    public void RemoveCard(SourceCard card)
    {
        PlayerDeck.Remove(card);
    }

    public void UpdateDeck(List<SourceCard> updatedDeck)
    {
        PlayerDeck = new List<SourceCard>(updatedDeck);
    }

    public PlayerSaveData ToSaveData()
    {
        return new PlayerSaveData(
            HeroBattler,
            PlayerDeck
        );
    }

    public void LoadFrom(PlayerSaveData data)
    {
        HeroBattler = data.HeroBattler;
        PlayerDeck = data.PlayerDeck;
    }
}