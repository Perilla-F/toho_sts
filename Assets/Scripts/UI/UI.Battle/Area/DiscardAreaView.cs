using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardAreaView : MonoBehaviour, IDiscardAreaView
{
    [SerializeField] private TextMeshProUGUI _discordCountText;

    public Transform GetTransform() => transform;

    public void UpdateDiscardCount(int count)
    {
        _discordCountText.text = count.ToString();
    }
}
