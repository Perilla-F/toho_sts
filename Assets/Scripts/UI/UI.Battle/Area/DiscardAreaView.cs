using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardAreaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _discordCountText;
    public DiscardArea DiscardArea;

    public void SetDiscardArea(DiscardArea discardArea)
    {
        DiscardArea = discardArea;
    }

    public void UpdateDiscardCount(string deckCount)
    {
        _discordCountText.text = deckCount;
    }
}
