using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeck
{
    private Queue<ICardObj> _deckCards = new Queue<ICardObj>();

    public event Action<int> OnChangedDeckCount;

    /// <summary>
    /// 山札Queue<CardObj>→Queue<CardObj>のシャッフル
    /// </summary>
    public void Shuffle()
    {
        // 1. QueueからListに取り出す
        List<ICardObj> tempList = new List<ICardObj>(_deckCards);
        _deckCards.Clear();

        // 2. Fisher-Yates シャッフルアルゴリズム
        // i: 最後の要素から2番目の要素まで
        for (int i = tempList.Count - 1; i > 0; i--)
        {
            // 0からi（両端を含む）の範囲でランダムなインデックス j を選択
            // Random.Range(int min, int max) は max が排他的（exclusive）なので、
            // i+1 を指定すると 0 から i まで（inclusive）になります。
            int j = UnityEngine.Random.Range(0, i + 1);

            // 要素を交換 (Swap)
            (tempList[i], tempList[j]) = (tempList[j], tempList[i]);
        }

        // 3. ListからQueueに戻す
        foreach (var card in tempList)
            _deckCards.Enqueue(card);
    }

    /// <summary>
    /// 現在の山札の残り枚数
    /// </summary>
    public int Count => _deckCards.Count;

    /// <summary>
    /// 山札に追加
    /// </summary>
    public void AddCard(ICardObj card)
    {
        _deckCards.Enqueue(card);
        OnChangedDeckCount?.Invoke(_deckCards.Count);
    }

    /// <summary>
    /// 山札からカードを1枚引く
    /// </summary>
    public ICardObj Draw()
    {
        var card = _deckCards.Dequeue();
        OnChangedDeckCount?.Invoke(_deckCards.Count);
        return card;
    }

    public List<SourceCard> GetDeckAsSourceCards()
    {
        return _deckCards.Select(card => card.GetSource()).ToList();
    }

    public bool IsEmpty()
    {
        return _deckCards.Count <= 0;
    }

    /// <summary>
    /// デッキの上位count枚を取り出す
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private List<ICardObj> PeekTopCards(Queue<ICardObj> deck, int count)
    {
        List<ICardObj> list = new List<ICardObj>(deck);
        return list.GetRange(0, Mathf.Min(count, list.Count));
    }

    /// <summary>
    /// 一枚のカードを上に置いて他をシャッフルする
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="selectedCard"></param>
    /// <returns></returns>
    private Queue<ICardObj> RebuildDeckWithTopCard(Queue<ICardObj> deck, ICardObj selectedCard)
    {
        List<ICardObj> deckList = new List<ICardObj>(deck);
        deckList.Remove(selectedCard); // 選んだカードを除く

        // シャッフル（Fisher-Yates）
        for (int i = deckList.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (deckList[i], deckList[j]) = (deckList[j], deckList[i]);
        }

        // 新しいデッキ構成：選んだカード + シャッフル済みカード
        Queue<ICardObj> newDeck = new Queue<ICardObj>();
        newDeck.Enqueue(selectedCard);
        foreach (var card in deckList)
        {
            newDeck.Enqueue(card);
        }

        return newDeck;
    }
}
