using System.Collections.Generic;

public class EnemyDefendAction : BattleAction
{
    public int amount;
    public EnemyDefendAction(EnemyUnit self, int amount, EnemyActionTarget targets, HeroUnit hero, EnemyManager enemies)
    {
        this.amount = amount;
        Targets = targets;
        Self = self;
        Hero = hero;
        Enemies = enemies;
    }

    public override void Execute()
    {
        List<IBattlerUnit> targets = new List<IBattlerUnit>();
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
        {
            foreach (var target in targets)
            {
                target.ApplyBlock(amount);
            }
        }
    }
}