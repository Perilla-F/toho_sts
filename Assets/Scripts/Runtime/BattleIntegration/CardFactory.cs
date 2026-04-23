using UnityEngine;

public class CardFactory
{
    private PlayerController _controller;

    public CardFactory(PlayerController controller)
    {
        _controller = controller;
    }

    /// <summary>
    /// SourceCard から CardObj と UI を生成
    /// </summary>
    public CardObj CreateCard(SourceCard sourceCard)
    {
        ResourceRegistry registry = new ResourceRegistry();
        UnityEngine.Debug.Log("OK!");

        // 論理データ生成
        CardObj cardObj = new NomalCardObj(sourceCard, registry, _controller);

        return cardObj;
    }

}
