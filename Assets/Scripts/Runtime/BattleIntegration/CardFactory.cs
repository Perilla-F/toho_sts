using UnityEngine;

public class CardFactory : ICardFactory
{
    private PlayerController _controller;
    private CardFactoryConfig _config;

    public CardFactory(CardFactoryConfig config, PlayerController controller)
    {
        _config = config;
        _controller = controller;
    }

    /// <summary>
    /// SourceCard から CardObj と UI を生成
    /// </summary>
    public CardObj CreateCard(SourceCard sourceCard, IBattleViewRoot view)
    {
        ResourceRegistry registry = new ResourceRegistry();

        // 見た目生成
        var behavior = Object.Instantiate(_config.CardViewPrefab, view.DeckView.GetTransform());

        // 双方向の初期化
        behavior.Init(view.DeckView, view.HandView, view.DiscardAreaView, view.TimelineView);

        // 論理データ生成
        CardObj cardObj = new NomalCardObj(sourceCard, registry, _controller);
        cardObj.BindView(behavior);
        behavior.BindCard(cardObj);
        behavior.gameObject.SetActive(false);

        return cardObj;
    }

}
