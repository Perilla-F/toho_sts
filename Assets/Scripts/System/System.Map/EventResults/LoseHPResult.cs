using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/LoseHP")]
public class LoseHPResult : EventResult
{
    public int amount = 10;
    public override void Apply(GameContext context, IFlagManager flagManager)
    {
        context.Player.HeroBattler.HPResource.LoseHP(amount);
        Debug.Log($"HP -{amount}");
    }
}
