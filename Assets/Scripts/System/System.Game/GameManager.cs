using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditorInternal;

public class GameManager
{
    private readonly ISaveService _saveService;
    private readonly ISceneLoader _sceneLoader;
    private readonly ISaveScheduler _saveScheduler;

    private readonly GameContext _context;

    public GameManager(ISceneLoader sceneLoader, ISaveService saveService, ISaveScheduler saveScheduler, GameContext context)
    {
        _sceneLoader = sceneLoader;
        _saveService = saveService;
        _saveScheduler = saveScheduler;
        _context = context;
    }

    public void InitializeGame()
    {
        Debug.Log("GameManager initialized!");
    }

    public void InitializePlayer(HeroData heroData)
    {
        HPResource hPResource = new HPResource(heroData.MaxHP);
        Mana mana = new Mana(heroData.MaxMana);

        SetHeroBattler(new HeroBattler(heroData, hPResource, mana));

        for (int i = 0; i < heroData.StartingDeck.Count; i++)
        {
            AddCard(new SourceCard(heroData.StartingDeck[i], hPResource, mana));
        }
    }

    public void SetHeroBattler(HeroBattler hero)
    {
        _context.SetHeroBattler(hero);
    }

    public void AddCard(SourceCard card)
    {
        _context.AddCard(card);
    }

    public void RemoveCard(SourceCard card)
    {
        _context.RemoveCard(card);
    }

    public void UpdateDeckAfterBattle(List<SourceCard> deck)
    {
        _context.UpdateDeck(deck);
    }

    public List<SourceCard> GetSourceCards()
    {
        return _context.GetSourceCards();
    }

    public void SelectCharacter(HeroData data)
    {
        InitializePlayer(data);
        _saveService.DeleteSave();
        _sceneLoader.LoadScene("MapScene");
    }

    public HeroBattler GetHeroBattler()
    {
        return _context.Hero;
    }

    public void StartBattle(EnemyType type)
    {
        var encounters = EncounterLoader.LoadEncounters(type, _context.StageIndex);
        var selected = encounters[UnityEngine.Random.Range(0, encounters.Count)];
        var data = new BattleTransitionData(_context.Hero, selected);
        _sceneLoader.SetTransitionData(data);
        _sceneLoader.LoadScene("BattleScene");
    }

    public void LoadGame()
    {
        var data = _saveService.LoadGame();
        if (data == null) return;

        _context.FromSaveData(data);
        _sceneLoader.LoadScene("MapScene");
    }

    public void RequestSave()
    {
        _saveScheduler.ScheduleSave(() =>
        {
            var data = _context.ToSaveData();
            _saveService.SaveGame(data);
        });
    }

}
