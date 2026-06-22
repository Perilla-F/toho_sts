using System;

public class ActionContext
{
    public IBattleUnit Self;
    public IBattleUnit Target;
    public BattleEvent BattleEvent;

    public ActionContext(IBattleUnit self, IBattleUnit target, BattleEvent battleEvent)
    {
        Self = self;
        Target = target;
        BattleEvent = battleEvent;
    }

}