using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour, IBattleSystem
{

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private StateManager stateManager;
    [SerializeField] private DiscardArea discardArea;
    private BattleDeck battleDeck;
    private Hand hand;
    public HeroBattler heroBattler;
    private HeroUnit heroUnit;
    private TimelineManager timelineManager;
    public BattleContext battleContext { get; private set; }

    public Hand Hand { get => hand; }
    public BattleDeck BattleDeck { get => battleDeck; }
    public DiscardArea DiscardArea { get => discardArea; }
    public HeroUnit HeroUnit { get => heroUnit; }
    public EnemyManager EnemyManager { get => enemyManager; }
    public TimelineManager TimelineManager { get => timelineManager; }

    public int TurnCount { get; private set; } = 1;

    public void Setup(BattleContext context, HeroUnit heroUnit, BattleDeck battleDeck)
    {
        battleContext = context;
        this.heroUnit = heroUnit;
        this.battleDeck = battleDeck;
        context.DeckView.UpdateDeckCount();

        stateManager.RegisterState(BattleStateType.SetUp, new BattleSetUpState(this));
        stateManager.RegisterState(BattleStateType.SetUp, new BattleStanbyState(this));
        stateManager.RegisterState(BattleStateType.Draw, new BattleDrawState(this));
        stateManager.RegisterState(BattleStateType.CardSelection, new BattleCardSelectionState(this));

        stateManager.ChangeState(BattleStateType.SetUp);

    }

    void Update()
    {
        stateManager.Update();
    }

    public void TransitionToState(BattleStateType nextState)
    {
        stateManager.ChangeState(nextState);
    }


    public void AddActionToTimeline()
    {
        foreach (var enemy in enemyManager.Enemies)
        {
            ConditionContext context = new ConditionContext(heroUnit);
            var actions = enemy.PlanTurn(TurnCount, context);
            foreach (var action in actions)
            {
                timelineManager.Enqueue(action);
            }
        }
    }

    public async UniTask Draw(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (battleDeck.IsEmpty()) break;
            CardObj card = battleDeck.Draw();
            hand.AddCard(card);
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
        battleContext.HandView.ArrangeCards();
        battleContext.DeckView.UpdateDeckCount();
    }

    /// <summary>
    /// 手札を全て捨て札へ送る
    /// </summary>
    /// <returns></returns>
    public async UniTask MoveAllToDiscard()
    {
        foreach (var card in hand.Cards)
        {
            discardArea.AddCard(card);
            await card.MoveDisCardAsync();
        }
        hand.Clear();
    }

    public async UniTask OnTurnEndButton()
    {
        TurnCount++;
        await MoveAllToDiscard();
        stateManager.ChangeState(BattleStateType.Draw);
    }

    public void EndBattle()
    {
        List<SourceCard> updatedDeck = battleDeck.GetDeckAsSourceCards();

        GameManager.Instance.UpdateAfterBattle(heroBattler, updatedDeck);

        SceneManager.LoadScene("MapScene");
    }
}
