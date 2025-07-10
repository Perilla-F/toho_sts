using System.Collections.Generic;

public class EnemyAttackAction : BattleAction
{
    public int damage;
    public EnemyAttackAction(EnemyUnit self, int dmg, EnemyActionTarget targets, HeroUnit hero, EnemyManager enemies)
    {
        damage = dmg;
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
        foreach (var target in targets)
        {
            target.TakeDamage(damage);
        }
    }
}