using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaView : MonoBehaviour
{
    [SerializeField] TextMeshPro _manaText;
    public Mana Mana { get; private set; }

    public void Init(Mana mana)
    {
        this.Mana = mana;
        mana.OnChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (Mana != null)
            Mana.OnChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        _manaText.text = Mana.GetMana().ToString();
    }
}
