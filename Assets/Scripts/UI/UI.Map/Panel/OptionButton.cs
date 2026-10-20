using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Button _button;

    public void Setup(EventOption option, Action<EventOption> onClick)
    {
        _button = GetComponent<Button>();
        text.text = option.Text;
        _button.onClick.RemoveAllListeners(); // 安全のためにクリア
        _button.onClick.AddListener(() => onClick?.Invoke(option));
    }
}