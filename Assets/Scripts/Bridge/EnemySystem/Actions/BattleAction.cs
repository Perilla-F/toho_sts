using System;
using System.Collections.Generic;

public abstract class BattleAction : IComparable<BattleAction>
{

    public float scheduledTime;
    public EnemyUnit Self;
    public HeroUnit Hero;
    public EnemyActionTarget Targets;
    public EnemyManager Enemies;

    public int CompareTo(BattleAction other)
    {
        return scheduledTime.CompareTo(other.scheduledTime);
    }

    public virtual void Execute()
    {
    }
}
