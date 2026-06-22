using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardAreaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _discordCountText;
    private IReadOnlyDiscardArea _discard;

    public void Setup(IReadOnlyDiscardArea discard)
    {
        _discard = discard;
        UpdateDiscardCount();
    }

    public Transform GetTransform() => transform;

    public void UpdateDiscardCount()
    {
        _discordCountText.text = _discard.Count.ToString();
    }
}
