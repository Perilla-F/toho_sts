using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/LoseHP")]
public class LoseHPResult : EventResult
{
    public int amount = 10;
    public override void Apply(IGameManager game, IFlagManager flagManager)
    {
        game.Context.HeroBattler.HPResource.LoseHP(amount);
        Debug.Log($"HP -{amount}");
    }
}
