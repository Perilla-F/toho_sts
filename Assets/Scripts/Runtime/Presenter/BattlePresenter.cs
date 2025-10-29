using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattlePresenter
{
    private readonly BattleSystem _battleSystem;
    private readonly BattleViewRoot _battleView;

    public BattlePresenter(BattleSystem battleSystem, BattleViewRoot battleView)
    {
        _battleSystem = battleSystem;
        _battleView = battleView;

        _battleSystem.Hand.OnChangedHand += OnChangedHand;
        _battleSystem.BattleDeck.OnChangedDeckCount += OnChangedDeckCount;
        _battleSystem.DiscardArea.OnChangedDiscardCount += OnChangedDiscardCount;
        _battleSystem.Hero.Mana.OnChanged += OnManaChanged;
    }

    private void OnChangedHand()
    {
        _battleView.HandView.ArrangeCards();
    }

    private void OnChangedDeckCount(int count)
    {
        _battleView.DeckView.UpdateDeckCount(count);
    }

    private void OnChangedDiscardCount(int count)
    {
        _battleView.DiscardAreaView.UpdateDiscardCount(count);
    }

    private void OnManaChanged()
    {
        _battleView.ManaView.UpdateUI(_battleSystem.Hero.Mana.GetMana());
    }

    private void OnTurnStart(int turnCount)
    {
        _battleView.TurnMessagePanel.ShowMessage($"{KanjiNumberConverteUtil.ConvertToKanjiWithUnits(turnCount)}巡目");
    }
}
