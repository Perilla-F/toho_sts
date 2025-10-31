using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private HeroDatabase heroDatabase;
    private HeroData _reimu;
    private HeroData _marisa;

    public event Action<HeroData> OnCharacterSelect;

    public void OnSelectReimu()
    {
        OnCharacterSelect?.Invoke(_reimu);
    }

    public void OnSelectMarisa()
    {
        OnCharacterSelect?.Invoke(_marisa);
    }
}
