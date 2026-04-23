using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [SerializeField] private Image blockOverlay;
    [SerializeField] private Image simpleBlockOverlay;

    [SerializeField] private float smoothSpeed = 10f;
    private float currentValue;

    private HPResource _resource;

    public void Bind(HPResource resource)
    {
        // 古いイベント購読を解除（再バインド対策）
        if (_resource != null)
            _resource.OnChanged -= UpdateUI;

        _resource = resource;

        // 新しいHPResourceにイベント登録
        _resource.OnChanged += UpdateUI;

        // 初回反映
        _slider.maxValue = _resource.MaxHP;
        UpdateUI();
    }

    private void OnDestroy()
    {
        // メモリリーク防止
        if (_resource != null)
            _resource.OnChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (_resource == null) return;

        float hpRatio = (float)_resource.GetHP() / _resource.MaxHP;
        _slider.value = _resource.GetHP();

        if (blockOverlay)
        {
            float blockRatio = (_resource.Block + _resource.SimpleBlock) / (float)_resource.MaxHP;
            blockOverlay.fillAmount = Mathf.Min(1f, hpRatio + blockRatio);
        }

        if (simpleBlockOverlay)
        {
            float simpleRatio = _resource.SimpleBlock / (float)_resource.MaxHP;
            simpleBlockOverlay.fillAmount = Mathf.Min(1f, hpRatio + simpleRatio);
        }

        if (_hpText)
            _hpText.text = $"{_resource.GetHP()}";
    }
}
