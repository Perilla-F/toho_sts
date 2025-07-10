using UnityEngine;

public static class CardFactory
{
    private static GameObject cardPrefab;

    /// <summary>
    /// カード生成前にプレハブを登録（初期化用）
    /// </summary>
    public static void Initialize(GameObject prefab)
    {
        cardPrefab = prefab;
    }

    /// <summary>
    /// SourceCard から CardObj と UI を生成
    /// </summary>
    public static CardObj CreateCard(SourceCard sourceCard, Transform parent, DeckView deckView, HandView handView, DiscardAreaView discardAreaView, Mana mana)
    {
        if (cardPrefab == null)
        {
            Debug.LogError("CardFactory: cardPrefabが設定されていません。Initialize()してください。");
            return null;
        }

        ResourceRegistry registry = new ResourceRegistry();

        // 論理データ生成
        CardObj cardObj = new NomalCardObj(sourceCard, registry, mana);

        // 見た目生成
        GameObject cardGO = Object.Instantiate(cardPrefab, parent);
        CardBehaviour behaviour = cardGO.GetComponent<CardBehaviour>();

        // 双方向の初期化
        behaviour.Init(cardObj, deckView, handView, discardAreaView);

        return cardObj;
    }

}
