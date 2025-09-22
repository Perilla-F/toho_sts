using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _hpText; // TextMeshProの場合は TMP_Text に変更

    public void SetHp(int currentHp, int maxHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;

        if (_hpText != null)
        {
            _hpText.text = $"{currentHp} / {maxHp}";
        }
    }
}
