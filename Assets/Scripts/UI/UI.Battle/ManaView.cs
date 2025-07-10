using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaView : MonoBehaviour
{
    [SerializeField] TextMeshPro manaText;
    public Mana mana { get; private set; }

    public void Init(Mana mana)
    {
        this.mana = mana;
        mana.OnChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (mana != null)
            mana.OnChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        manaText.text = mana.GetMana().ToString();
    }
}
