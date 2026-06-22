using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _manaText;

    private IReadOnlyMana _mana;

    public void Setup(IReadOnlyMana mana)
    {
        _mana = mana;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _manaText.text = _mana.CurrentResource.ToString();
    }
}
