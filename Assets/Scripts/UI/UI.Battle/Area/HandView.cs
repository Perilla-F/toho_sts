using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class HandView : MonoBehaviour, IHandView
{
    public Hand Hand { get; private set; }
    private List<Behaviour> _cards = new List<Behaviour>();

    public void SetHand(Hand hand)
    {
        Hand = hand;
    }

    // 手札を整列させる
    public void ArrangeCards()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            float center = (_cards.Count - 1) / 2.0f;
            float interval = 100.0f;
            float x = (i - center) * interval;
            _cards[i].transform.localPosition = new Vector3(x, 0, 0);
        }
    }

}
