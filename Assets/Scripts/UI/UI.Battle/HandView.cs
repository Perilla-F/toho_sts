using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class HandView : MonoBehaviour, IHandView
{
    public Hand hand { get; private set; }
    List<Behaviour> cards = new List<Behaviour>();

    public void SetHand(Hand hand)
    {
        this.hand = hand;
    }

    // 手札を整列させる
    public void ArrangeCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            float center = (cards.Count - 1) / 2.0f;
            float interval = 100.0f;
            float x = (i - center) * interval;
            cards[i].transform.localPosition = new Vector3(x, 0, 0);
        }
    }

}
