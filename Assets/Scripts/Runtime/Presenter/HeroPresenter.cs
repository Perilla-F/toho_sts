using Live2D.Cubism.Framework.Motion;

public class HeroPresenter
{
    private readonly HeroUnit _heroUnit;
    private readonly HeroModel _model;

    public HeroPresenter(HeroUnit heroUnit, IHeroModel model)
    {
        _heroUnit = heroUnit;
        _model = model;

        // System層 → UI層 通知
        _heroUnit.OnHPChanged += OnAttack();
        _heroUnit.OnManaChanged += OnHit();
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

}
