using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckViewController : MonoBehaviour
{
    // デッキのデータ（Queue）

    [SerializeField] private Transform DeckListContentParent;
    [SerializeField] private Transform DiscardListContentParent;
    [SerializeField] private GameObject cardUIPrefab;         // カードを表示するUIのプレハブ

    private IReadOnlyBattleContext _context;

    public void Setup(IReadOnlyBattleContext context)
    {
        _context = context;
    }

    // デッキアイコンが押されたときの処理
    public void OnClickDeckIcon()
    {
        ShowDeckList();
    }

    /// <summary>
    /// デッキ内のカードをソートしてパネルに並べ替えて表示する
    /// </summary>
    public void ShowDeckList()
    {
        // 1. 既存の表示をクリア
        foreach (Transform child in DeckListContentParent)
        {
            Destroy(child.gameObject);
        }

        // 2. カードをソート
        IReadOnlyList<IReadOnlyCardObj> sortedCards = _context.Deck.DeckCards
        .OrderBy(c => c.Source.Data.Hero)
        .ThenBy(c => c.Source.Data.Costs.Find(d => d.Type == ResourceType.Mana).Amount)
        .ThenBy(c => c.Source.Data.Rarity)
        .ThenBy(c => c.Source.Data.CardID)
        .ThenBy(c => c.Source.UpgradedLevel).ToList();

        // 3. 順番にインスタンス化してGrid Layout Groupに配置する
        foreach (var cardObj in sortedCards)
        {
            GameObject cardObjUI = Instantiate(cardUIPrefab, DeckListContentParent);

            // データを流し込む
            CardUIComponent uiComp = cardObjUI.GetComponent<CardUIComponent>();
            uiComp.SetupForBattleViewer(cardObj, _context);
        }
    }

    public void OnClickDiscardIcon()
    {
        ShowDiscardList();
    }

    /// <summary>
    /// デッキ内のカードをソートしてパネルに並べ替えて表示する
    /// </summary>
    public void ShowDiscardList()
    {
        // 1. 既存の表示をクリア
        foreach (Transform child in DiscardListContentParent)
        {
            Destroy(child.gameObject);
        }

        // 2. カードをソート
        IReadOnlyList<IReadOnlyCardObj> sortedCards = _context.Discard.DiscardedCards
        .OrderBy(c => c.Source.Data.Hero)
        .ThenBy(c => c.Source.Data.Costs.Find(d => d.Type == ResourceType.Mana).Amount)
        .ThenBy(c => c.Source.Data.Rarity)
        .ThenBy(c => c.Source.Data.CardID)
        .ThenBy(c => c.Source.UpgradedLevel).ToList();

        // 3. 順番にインスタンス化してGrid Layout Groupに配置する
        foreach (var cardObj in sortedCards)
        {
            GameObject cardObjUI = Instantiate(cardUIPrefab, DiscardListContentParent);

            // データを流し込む
            CardUIComponent uiComp = cardObjUI.GetComponent<CardUIComponent>();
            uiComp.SetupForBattleViewer(cardObj, _context);
        }
    }
}