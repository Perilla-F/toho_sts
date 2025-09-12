using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Conditions/Turn")]
public class TurnCondition : EnemyCondition
{
    public enum TurnType { Exact, MultipleOf, After }
    public TurnType turnType;
    public int value;

    public override bool IsSatisfied(IBattleContext context, IBattleUnit self)
    {
        int turn = context.Turn;
        return turnType switch
        {
            TurnType.Exact => turn == value,
            TurnType.MultipleOf => turn % value == 0,
            TurnType.After => turn >= value,
            _ => false
        };
    }
}