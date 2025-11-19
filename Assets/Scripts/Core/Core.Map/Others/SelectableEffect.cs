using UnityEngine;
using UnityEngine.UI;

public class SelectableEffect : MonoBehaviour
{
    private Image _img;
    void Start() => _img = GetComponent<Image>();

    void Update()
    {
        if (_img == null) return;
        float alpha = Mathf.PingPong(Time.time, 1f);
        _img.color = new Color(1f, 1f, 0.3f, 0.5f + alpha * 0.5f);
    }
}
