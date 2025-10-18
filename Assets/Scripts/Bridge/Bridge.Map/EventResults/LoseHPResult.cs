using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/LoseHP")]
public class LoseHPResult : EventResult
{
    public int amount;
    public override void Apply()
    {
        GameManager.Instance.HeroBattler.HPResource.Gain(-amount);
        Debug.Log($"HP -{amount}");
    }
}
