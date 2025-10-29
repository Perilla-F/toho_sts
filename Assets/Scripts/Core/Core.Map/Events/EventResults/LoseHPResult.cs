using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/LoseHP")]
public class LoseHPResult : EventResult
{
    public int amount = 10;
    public override void Apply(IGameContext context, IFlagManager flagManager)
    {
        context.Player.HeroBattler.HPResource.LoseHP(amount);
        Debug.Log($"HP -{amount}");
    }
}
