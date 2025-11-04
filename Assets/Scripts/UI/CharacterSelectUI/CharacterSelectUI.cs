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
        _reimu = heroDatabase.GetHeroData("reimu");
        OnCharacterSelect?.Invoke(_reimu);
    }

    public void OnSelectMarisa()
    {
        _marisa = heroDatabase.GetHeroData("marisa");
        OnCharacterSelect?.Invoke(_marisa);
    }
}
