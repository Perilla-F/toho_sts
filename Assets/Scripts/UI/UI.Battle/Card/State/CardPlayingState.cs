using UnityEngine;
using DG.Tweening;

public class CardPlayingState : CardStateBase
{
    public override bool CanDrag => false;

    public CardPlayingState(BattleCard owner) : base(owner)
    {
    }

    public override void OnEnter()
    {
        _owner.transform.DOScale(new Vector3(3, 3, 3), 0.2f).SetEase(Ease.OutCubic);
    }

    public override void OnExit()
    {
        _owner.ChangeState(new CardBusyState(_owner));
    }
}