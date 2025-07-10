using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardAreaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI discordCountText;
    public void UpdateDiscardCount(string deckCount)
    {
        discordCountText.text = deckCount;
    }
}
