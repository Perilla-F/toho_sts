using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour, IBattleSystem
{

    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private StateManager _stateManager;
    [SerializeField] private DiscardArea _discardArea;
    private BattleDeck _battleDeck;
    private Hand _hand;
    public HeroBattler HeroBattler;
    private HeroUnit _heroUnit;
    private TimelineManager _timelineManager;
    public BattleContext BattleContext { get; private set; }

    public Hand Hand { get => _hand; }
    public BattleDeck BattleDeck { get => _battleDeck; }
    public DiscardArea DiscardArea { get => _discardArea; }
    public HeroUnit HeroUnit { get => _heroUnit; }
    public EnemyManager EnemyManager { get => _enemyManager; }
    public TimelineManager TimelineManager { get => _timelineManager; }

    public int TurnCount { get; private set; } = 1;

    public void Setup(BattleContext context, HeroUnit heroUnit, BattleDeck battleDeck)
    {
        BattleContext = context;
        this._heroUnit = heroUnit;
        this._battleDeck = battleDeck;
        context.DeckView.UpdateDeckCount();

        _stateManager.RegisterState(BattleStateType.SetUp, new BattleSetUpState(this));
        _stateManager.RegisterState(BattleStateType.SetUp, new BattleStanbyState(this));
        _stateManager.RegisterState(BattleStateType.Draw, new BattleDrawState(this));
        _stateManager.RegisterState(BattleStateType.CardSelection, new BattleCardSelectionState(this));

        _stateManager.ChangeState(BattleStateType.SetUp);

    }

    void Update()
    {
        _stateManager.Update();
    }

    public void TransitionToState(BattleStateType nextState)
    {
        _stateManager.ChangeState(nextState);
    }


    public void AddActionToTimeline()
    {
        foreach (var enemy in _enemyManager.Enemies)
        {
            ConditionContext context = new ConditionContext(_heroUnit);
            var actions = enemy.PlanTurn(TurnCount, context);
            foreach (var action in actions)
            {
                _timelineManager.Enqueue(action);
            }
        }
    }

    public async UniTask Draw(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_battleDeck.IsEmpty()) break;
            CardObj card = _battleDeck.Draw();
            _hand.AddCard(card);
            if (i < count - 1)
            {
                card.MoveCardAsync().Forget();
                await UniTask.Delay(100);
            }
            else
            {
                await card.MoveCardAsync();
            }
        }
        BattleContext.HandView.ArrangeCards();
        BattleContext.DeckView.UpdateDeckCount();
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask MoveAllToDiscard()
    {
        foreach (var card in _hand.Cards)
        {
            _discardArea.AddCard(card);
            await card.MoveDisCardAsync();
        }
        _hand.Clear();
    }

    public async UniTask OnTurnEndButton()
    {
        TurnCount++;
        await MoveAllToDiscard();
        _stateManager.ChangeState(BattleStateType.Draw);
    }

    public void EndBattle()
    {
        List<SourceCard> updatedDeck = _battleDeck.GetDeckAsSourceCards();

        GameManager.Instance.UpdateAfterBattle(HeroBattler, updatedDeck);

        SceneManager.LoadScene("MapScene");
    }
}
