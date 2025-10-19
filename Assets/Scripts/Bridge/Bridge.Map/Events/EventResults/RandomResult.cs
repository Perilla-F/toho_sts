using UnityEngine;

[CreateAssetMenu(menuName = "Events/Results/Random")]
public class RandomResult : EventResult
{
    [Range(0f, 1f)] public float successRate = 0.5f;
    public EventResult success;
    public EventResult fail;

    public override void Apply()
    {
        if (Random.value <= successRate)
        {
            Debug.Log("Random Success!");
            success?.Apply();
        }
        else
        {
            Debug.Log("Random Fail!");
            fail?.Apply();
        }
    }
}
