using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("Card UI Elements")]
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private Image _cardFrame;
    [SerializeField] private Image _artwork;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _delayText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private CardData _cardData;

    /// <summary>
    /// 外部からCardDataを設定し、UIを更新する
    /// </summary>
    public void SetCard(CardData cardData)
    {
        _cardData = cardData;
        UpdateUI();
    }

    /// <summary>
    /// CardDataの内容をUI要素に反映
    /// </summary>
    private void UpdateUI()
    {
        if (_cardData == null)
        {
            Debug.LogWarning("CardData is null!");
            return;
        }

        _cardName.text = _cardData.CardName;
        _artwork.sprite = _cardData.Artwork;
        _descriptionText.text = _cardData.Description;
        _delayText.text = _cardData.Delay.ToString();

        if (_cardData.Costs != null && _cardData.Costs.Count > 0)
        {
            int cost = 0;
            foreach (var c in _cardData.Costs)
            {
                if (c.Type == ResourceType.Mana)
                {
                    cost = c.Amount;
                }
            }
            _costText.text = cost.ToString();
        }
        else
        {
            _costText.text = "-";
        }

        switch (_cardData.Rarity)
        {
            case CardRarity.Common:
                _cardFrame.color = Color.white;
                break;
            case CardRarity.Uncommon:
                _cardFrame.color = Color.blue;
                break;
            case CardRarity.Rare:
                _cardFrame.color = Color.yellow;
                break;
        }
    }
}
