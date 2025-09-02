using UnityEngine;

public class SpriteOrderExample : MonoBehaviour
{
    public SpriteRenderer Background;
    public SpriteRenderer Icon;
    public SpriteRenderer Current;
    public SpriteRenderer SelectableEffect;

    void Start()
    {
        Background.sortingOrder = 0; // 後ろに描画
        Icon.sortingOrder = 1;       // 前に描画
        Current.sortingOrder = 2;
        SelectableEffect.sortingOrder = 3;
    }
}
