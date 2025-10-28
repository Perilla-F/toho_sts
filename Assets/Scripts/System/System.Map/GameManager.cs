using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditorInternal;

public class GameManager : IGameManager
{
    private readonly ISaveService _saveService;
    private readonly ISceneLoader _sceneLoader;
    private readonly ISaveScheduler _saveScheduler;

    private readonly GameContext _context;

    public event Action<SaveData> OnSaveRequested;

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
        _context.Player.InitializePlayer(heroData);
    }

    public void AddCard(SourceCard card)
    {
        _context.Player.AddCard(card);
    }

    public void RemoveCard(SourceCard card)
    {
        _context.Player.RemoveCard(card);
    }

    public void UpdateDeckAfterBattle(List<SourceCard> deck)
    {
        _context.Player.UpdateDeck(deck);
    }

    public void StartBattle(EnemyType type)
    {
        var encounters = EncounterLoader.LoadEncounters(type, _context.Map.StageIndex);
        var selected = encounters[UnityEngine.Random.Range(0, encounters.Count)];
        var data = new BattleTransitionData(_context.Player.HeroBattler, selected);
        _sceneLoader.SetTransitionData(data);
        _sceneLoader.LoadScene("BattleScene");
    }

    public void LoadGame()
    {
        var data = _saveService.LoadGame();
        if (data == null) return;

        _context.FromSaveData(data);
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
