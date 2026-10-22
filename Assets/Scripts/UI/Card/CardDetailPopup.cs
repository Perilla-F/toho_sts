using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDetailPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CardUIComponent cardComp;     // 左側の拡大表示用親
    [SerializeField] private TextMeshProUGUI effectText;      // キーワード効果テキスト
    [SerializeField] private TextMeshProUGUI flavorText;      // フレーバーテキスト
    [SerializeField] private Toggle enhanceCheckbox;          // 下の強化状態チェックボックス
    [SerializeField] private Button closeButton;              // 右上の×スイッチ
    [SerializeField] private GameObject backgroundDimmer;     // 背景を暗くするパネル
    [SerializeField] private GameObject cardUIPrefab;

    private CardData currentData;
    private GameObject spawnedPreviewCard;

    private void Awake()
    {
        // 閉じるボタンのイベント登録
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePopup);
        }

        // 強化チェックボックスの切り替えイベント登録
        if (enhanceCheckbox != null)
        {
            enhanceCheckbox.onValueChanged.AddListener(OnEnhanceToggleChanged);
        }
    }

    public void Show(CardData data)
    {
        currentData = data;

        // 1. UIを前列（最前面）に移動する
        transform.SetAsLastSibling();

        // 2. 左右の情報をセット
        enhanceCheckbox.isOn = data.IsUpgraded;
        RefreshContent();

        // 3. ポップアップ全体と背景を表示状態にする
        gameObject.SetActive(true);
        if (backgroundDimmer != null) backgroundDimmer.SetActive(true);
    }

    private void RefreshContent()
    {
        if (currentData != null)
        {
            UpdateCardPreview();

            effectText.text = BuildKeywordDetailsText(currentData.CardEffects);
            flavorText.text = currentData.FlavorText;
        }
    }

    /// <summary>
    /// CardEffectInstanceのリストから KeywordDetail を抽出し、改行で結合する
    /// </summary>
    private string BuildKeywordDetailsText(List<CardEffectInstance> cardEffects)
    {
        if (cardEffects == null || cardEffects.Count == 0)
        {
            return string.Empty;
        }

        var details = cardEffects
            .Where(e => e.Definition != null)
            .Select(e => e.Definition.KeywordDetail)
            .Where(detail => !string.IsNullOrEmpty(detail))
            .Distinct();

        return string.Join("\n\n", details);
    }

    private void OnEnhanceToggleChanged(bool isOn)
    {
        currentData = currentData.UpgradedCardData;

        // チェックボックスが切り替わったときの再描画
        RefreshContent();
    }

    private void UpdateCardPreview()
    {
        // 既存のプレビューがあれば破棄して作り直す（または使い回す）
        if (spawnedPreviewCard != null)
        {
            Destroy(spawnedPreviewCard);
        }

        if (cardComp != null)
        {
            // インスタンスがあるならインスタンス版、なければマスターデータ版で表示
            if (currentData != null)
            {
                cardComp.SetupForDeckMenu(currentData);
            }
        }
        cardComp.IsInspectable = false;

    }

    public void ClosePopup()
    {
        // プレビュー用カードを掃除
        if (spawnedPreviewCard != null)
        {
            Destroy(spawnedPreviewCard);
            spawnedPreviewCard = null;
        }

        // 非表示にする
        gameObject.SetActive(false);
    }
}