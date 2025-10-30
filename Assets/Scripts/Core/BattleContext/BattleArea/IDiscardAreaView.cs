using UnityEngine;

public interface IDiscardAreaView
{
    public Transform GetTransform();
    public void UpdateDiscardCount(int count);
}
