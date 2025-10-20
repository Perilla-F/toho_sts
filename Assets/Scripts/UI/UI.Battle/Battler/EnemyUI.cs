using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour, IBattleUI
{
    [Header("UI References")]
    [SerializeField] private HpBar hpBar;
    [SerializeField] private Transform buffContainer;
    [SerializeField] private Image actionIcon;
    [SerializeField] private TMP_Text actionNameText;

    [SerializeField] private Image intentIcon;
    [SerializeField] private Transform highlightEffect;

    private Vector3 _baseScale;

    private void Start()
    {
        _baseScale = actionIcon.transform.localScale;
        SetActionIconVisible(false);
    }

    public void Bind(HPResource resource)
    {
        hpBar.Bind(resource);
    }

    public void SetActionIcon(Sprite sprite, string actionName = "")
    {
        if (actionIcon == null) return;
        actionIcon.sprite = sprite;
        actionNameText.text = actionName;
        SetActionIconVisible(true);
    }

    public void SetActionIconVisible(bool visible)
    {
        if (actionIcon != null) actionIcon.enabled = visible;
        if (actionNameText != null) actionNameText.enabled = visible;
    }

    public void HighlightAction(bool highlight)
    {
        if (actionIcon == null) return;
        actionIcon.transform.localScale = highlight ? _baseScale * 1.3f : _baseScale;
    }

    public void ShowIntentIcon(Sprite icon)
    {
        intentIcon.sprite = icon;
        intentIcon.gameObject.SetActive(true);
    }

    public void Highlight(bool active)
    {
        if (highlightEffect)
            highlightEffect.gameObject.SetActive(active);
    }

    public void ShowDamageEffect(float duration)
    {
        // 被ダメージエフェクト処理など
    }
}
