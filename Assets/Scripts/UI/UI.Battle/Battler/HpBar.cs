using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HpBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _maxHpText;
    [SerializeField] private TextMeshProUGUI _blockText;
    [SerializeField] private TextMeshProUGUI _simpleBlockText;

    [SerializeField] private float smoothSpeed = 0.3f;
    [SerializeField] private HorizontalLayoutGroup _hpBox;
    [SerializeField] private LayoutElement _hpBar;
    [SerializeField] private LayoutElement _lostHpBar;
    [SerializeField] private LayoutElement _blockBar;
    [SerializeField] private LayoutElement _simpleBlockBar;

    private float _targetHpWidth;
    private float _targetLostHpWidth;
    private float _targetBlockWidth;
    private float _targetSimpleBlockWidth;

    public void ApplyVisuals(IReadOnlyBattleUnit unit)
    {
        var resource = unit.HPResource;
        var defenseComponent = unit.DefenseComponent;

        var currentHP = resource.GetHP();
        var maxHP = resource.MaxHP;
        var block = defenseComponent.Block;
        var simpleBlock = defenseComponent.SimpleBlock;

        int total = currentHP + block + simpleBlock;
        int baseWidth = Mathf.Max(maxHP, total);

        float totalWidth = _hpBox.GetComponent<RectTransform>().rect.width;

        _hpText.text = currentHP.ToString();
        _maxHpText.text = maxHP.ToString();
        _blockText.text = block.ToString();
        _simpleBlockText.text = simpleBlock.ToString();


        _targetHpWidth = (float)currentHP / baseWidth * totalWidth;
        _hpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _targetHpWidth);
        _targetLostHpWidth = (float)(maxHP - currentHP) / baseWidth * totalWidth;
        _lostHpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _targetLostHpWidth);
        _targetBlockWidth = (float)block / baseWidth * totalWidth;
        _blockBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _targetBlockWidth);
        _targetSimpleBlockWidth = (float)simpleBlock / baseWidth * totalWidth;
        _simpleBlockBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _targetSimpleBlockWidth);

        DOTween.To(() => _hpBar.preferredWidth, x => _hpBar.preferredWidth = x, _targetHpWidth, smoothSpeed);
        DOTween.To(() => _lostHpBar.preferredWidth, x => _lostHpBar.preferredWidth = x, _targetLostHpWidth, smoothSpeed);
        DOTween.To(() => _blockBar.preferredWidth, x => _blockBar.preferredWidth = x, _targetBlockWidth, smoothSpeed);
        DOTween.To(() => _simpleBlockBar.preferredWidth, x => _simpleBlockBar.preferredWidth = x, _targetSimpleBlockWidth, smoothSpeed);

        Debug.Log($"TotalWidth: {totalWidth}, BaseWidth: {baseWidth}, HPBarWidth: {_hpBar.preferredWidth}");
    }

}
