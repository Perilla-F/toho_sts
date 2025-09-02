using System.Collections.Generic;

public class EnemyAttackBuffAction : BattleAction
{
    public int Amount;
    public EnemyAttackBuffAction(EnemyUnit self, int amount, EnemyActionTarget targets, HeroUnit hero, EnemyManager enemies)
    {
        this.Amount = amount;
        Targets = targets;
        Self = self;
        Hero = hero;
        Enemies = enemies;
    }

    public override void Execute()
    {
        List<IBattleUnit> targets = new List<IBattleUnit>();
        switch (Targets)
        {
            case EnemyActionTarget.Self:
                targets.Add(Self);
                break;
            case EnemyActionTarget.Hero:
                targets.Add(Hero);
                break;
            case EnemyActionTarget.Member:
                targets.Add(GetRandomFromList.GetRandom(Enemies.Enemies));
                break;
            case EnemyActionTarget.Group:
                foreach (var enemy in Enemies.Enemies)
                {
                    targets.Add(enemy);
                }
                break;
        }
        foreach (var target in targets)
        {
            target.ApplyAttackBuff(Amount);
        }
    }
}