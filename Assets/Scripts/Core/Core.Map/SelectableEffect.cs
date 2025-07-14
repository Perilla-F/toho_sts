using UnityEngine;
using UnityEngine.UI;

public class SelectableEffect : MonoBehaviour
{
    private Image img;
    void Start() => img = GetComponent<Image>();

    void Update()
    {
        if (img == null) return;
        float alpha = Mathf.PingPong(Time.time, 1f);
        img.color = new Color(1f, 1f, 0.3f, 0.5f + alpha * 0.5f);
    }
}
