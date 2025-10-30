using Live2D.Cubism.Framework.Motion;
using UnityEngine;

public class HeroPresenter
{
    private readonly HeroUnit _heroUnit;
    private readonly HeroModel _model;

    private readonly HeroUI _heroUI;
    private readonly ManaView _manaView;

    public HeroPresenter(HeroUnit heroUnit, HeroModel model, HeroUI heroUI, ManaView manaView)
    {
        _heroUnit = heroUnit;
        _model = model;
        _heroUI = heroUI;
        _manaView = manaView;

        // System層 → UI層 通知
        _heroUnit.OnAttack += OnAttack;
        _heroUnit.OnHit += OnHit;
        _heroUnit.OnHpChanged += HPChange;
        _heroUnit.OnManaChanged += ManaChange;
    }

    // UI層 → System層 命令
    public void OnAttack(AnimationClip attack)
    {
        _model.PlayAttack(attack);
    }

    public void OnHit(AnimationClip hit)
    {
        _model.PlayHit(hit);
    }

    public void HPChange(int value)
    {
        _heroUI.UpdateHp(value);
    }

    public void ManaChange(int value)
    {
        _manaView.UpdateUI(value);
    }

}
