using UnityEngine;

public class CardFactory : MonoBehaviour
{
    public static CardFactory Instance { get; private set; }
    [SerializeField] private GameObject _cardPrefab;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// SourceCard から CardObj と UI を生成
    /// </summary>
    public CardObj CreateCard(SourceCard sourceCard, BattleViewRoot view)
    {
        if (_cardPrefab == null)
        {
            Debug.LogError("CardFactory: cardPrefabが設定されていません。Initialize()してください。");
            return null;
        }

        ResourceRegistry registry = new ResourceRegistry();

        // 論理データ生成
        CardObj cardObj = new NomalCardObj(sourceCard, registry);

        // 見た目生成
        GameObject cardGO = Instantiate(_cardPrefab, view.DeckView.transform);
        CardBehavior behaviour = cardGO.GetComponent<CardBehavior>();

        // 双方向の初期化
        behaviour.Init(cardObj, view.DeckView, view.HandView, view.DiscardAreaView, view.TimelineView);

        return cardObj;
    }

}
