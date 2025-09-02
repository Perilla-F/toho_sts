using System;
using System.Collections.Generic;

public class BattleAction : IComparable<BattleAction>
{
    public EnemyActionData Action;
    public float ScheduledTime;
    public EnemyUnit Self;
    public HeroUnit Hero;
    public EnemyActionTarget Targets;
    public EnemyManager Enemies;
    public BattleAction(EnemyActionData action)
    {
        Action = action;
    }

    public int CompareTo(BattleAction other)
    {
        return ScheduledTime.CompareTo(other.ScheduledTime);
    }

    public virtual void Execute()
    {
        switch (Action.ActionType)
        {
            case EnemyActionType.Attack:
                DoAttack(Action);
                ApplyEffects(Action);
                break;
        }
    }

    private void DoAttack(EnemyActionData action)
    {
        var target = action.Target;
        if (target == EnemyActionTarget.Hero)
        {
            Hero.TakeDamage(action.Value);
        }
    }

    private void ApplyEffects(EnemyActionData action)
    {
        foreach (var effectData in action.Effects)
        {
            var effect = EffectFactory.CreateEffect(effectData.EffectType, effectData.Amount);
            if (effect == null) continue;

            ApplyEffectToTarget(effectData.Target, effect);
        }
    }

    private void ApplyEffectToTarget(EnemyActionTarget targetType, IEffect effect)
    {
        switch (targetType)
        {
            case EnemyActionTarget.Hero:
                Hero.ApplyEffect(effect);
                break;

            case EnemyActionTarget.Self:
                Self.ApplyEffect(effect);
                break;

            case EnemyActionTarget.Group:
                foreach (var target in Enemies.Enemies)
                {
                    target.ApplyEffect(effect);
                }
                break;

            case EnemyActionTarget.Member:
                GetRandomFromList.GetRandom(Enemies.Enemies).ApplyEffect(effect);
                break;
        }
    }

}
