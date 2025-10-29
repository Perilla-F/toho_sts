using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardAreaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _discordCountText;

    public void UpdateDiscardCount(int count)
    {
        _discordCountText.text = count.ToString();
    }
}
