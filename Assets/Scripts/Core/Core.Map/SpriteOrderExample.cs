using UnityEngine;

public class SpriteOrderExample : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer icon;
    public SpriteRenderer current;
    public SpriteRenderer selectableEffect;

    void Start()
    {
        background.sortingOrder = 0; // 後ろに描画
        icon.sortingOrder = 1;       // 前に描画
        current.sortingOrder = 2;
        selectableEffect.sortingOrder = 3;
    }
}
