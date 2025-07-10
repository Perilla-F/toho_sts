using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeck : IBattleDeck
{
    private Queue<CardObj> deckCards = new Queue<CardObj>();

    /// <summary>
    /// 山札Queue<CardObj>→Queue<CardObj>のシャッフル
    /// </summary>
    public void Shuffle()
    {
        List<CardObj> tempList = new List<CardObj>(deckCards);
        deckCards.Clear();

        for (int i = 0; i < tempList.Count; i++)
        {
            var temp = tempList[i];
            int randomIndex = Random.Range(i, tempList.Count);
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }

        foreach (var card in tempList)
            deckCards.Enqueue(card);
    }

    /// <summary>
    /// 現在の山札の残り枚数
    /// </summary>
    public int Count => deckCards.Count;

    /// <summary>
    /// 山札に追加
    /// </summary>
    public void AddCard(CardObj card) => deckCards.Enqueue(card);

    /// <summary>
    /// 山札からカードを1枚引く
    /// </summary>
    public CardObj Draw()
    {
        return deckCards.Dequeue();
    }

    public List<SourceCard> GetDeckAsSourceCards()
    {
        return deckCards.Select(card => card.Source()).ToList();
    }

    public bool IsEmpty()
    {
        return deckCards.Count <= 0;
    }
}
