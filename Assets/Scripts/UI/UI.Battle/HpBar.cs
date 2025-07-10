using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText; // TextMeshProの場合は TMP_Text に変更

    public void SetHp(int currentHp, int maxHp)
    {
        slider.maxValue = maxHp;
        slider.value = currentHp;

        if (hpText != null)
        {
            hpText.text = $"{currentHp} / {maxHp}";
        }
    }
}
