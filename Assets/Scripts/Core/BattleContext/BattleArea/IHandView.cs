using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IHandView
{
    public Transform GetTransform();

    /// <summary>
    /// 手札を整列させる
    /// </summary>
    public void ArrangeCards();
}
