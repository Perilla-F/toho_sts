using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattlePresenter
{
    private readonly BattleSystem _battleSystem;
    private readonly BattleViewRoot _battleView;
    private readonly PlayerController _player;

    private readonly TurnEndButton _turnEndButton;

    public BattlePresenter(BattleSystem battleSystem, BattleViewRoot battleView, PlayerController player)
    {
        _battleSystem = battleSystem;
        _battleView = battleView;
        _player = player;

        _battleSystem.BattleStart += BattleStart;
        _battleSystem.OnTurnStart += TurnStart;

        _battleSystem.BattleContext.Hand.OnChangedHand += ArrangeHand;
        _battleSystem.BattleContext.Deck.OnChangedDeckCount += OnChangedDeckCount;
        _battleSystem.BattleContext.Discard.OnChangedDiscardCount += OnChangedDiscardCount;
        _battleSystem.Hero.Mana.OnChanged += OnManaChanged;

        _turnEndButton = _battleView.TurnEndButton;
        _turnEndButton.OnClickTurnEnd += OnClickTurnEnd;
    }

    private void BattleStart()
    {
        _battleView.BattleStart();
    }

    private void TurnStart(int turn)
    {
        _battleView.TurnStart(turn);
    }

    private void ArrangeHand()
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

    private async Task OnTurnStart(int turnCount)
    {
        await _battleView.TurnMessagePanel.ShowMessage($"{KanjiNumberConverteUtil.ConvertToKanjiWithUnits(turnCount)}巡目");
    }

    private void OnClickTurnEnd()
    {
        _player.TurnEndButton();
    }
}
