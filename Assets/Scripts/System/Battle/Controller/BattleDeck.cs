using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeck : IBattleDeck
{
    private Queue<CardObj> _deckCards = new Queue<CardObj>();

    public event Action<int> OnChangedDeckCount;

    /// <summary>
    /// 山札Queue<CardObj>→Queue<CardObj>のシャッフル
    /// </summary>
    public void Shuffle()
    {
        List<CardObj> tempList = new List<CardObj>(_deckCards);
        _deckCards.Clear();

        for (int i = 0; i < tempList.Count; i++)
        {
            var temp = tempList[i];
            int randomIndex = UnityEngine.Random.Range(i, tempList.Count);
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }

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
    public void AddCard(CardObj card)
    {
        _deckCards.Enqueue(card);
        OnChangedDeckCount?.Invoke(_deckCards.Count);
    }

    /// <summary>
    /// 山札からカードを1枚引く
    /// </summary>
    public CardObj Draw()
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

    List<CardObj> PeekTopCards(Queue<CardObj> deck, int count)
    {
        List<CardObj> list = new List<CardObj>(deck);
        return list.GetRange(0, Mathf.Min(count, list.Count));
    }

    Queue<CardObj> RebuildDeckWithTopCard(Queue<CardObj> deck, CardObj selectedCard)
    {
        List<CardObj> deckList = new List<CardObj>(deck);
        deckList.Remove(selectedCard); // 選んだカードを除く

        // シャッフル（Fisher-Yates）
        for (int i = deckList.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (deckList[i], deckList[j]) = (deckList[j], deckList[i]);
        }

        // 新しいデッキ構成：選んだカード + シャッフル済みカード
        Queue<CardObj> newDeck = new Queue<CardObj>();
        newDeck.Enqueue(selectedCard);
        foreach (var card in deckList)
        {
            newDeck.Enqueue(card);
        }

        return newDeck;
    }
}
