using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _manaText;

    public void UpdateUI(int count)
    {
        _manaText.text = count.ToString();
    }
}
