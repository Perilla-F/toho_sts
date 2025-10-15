using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour, IBattleUI
{
    [SerializeField] private Slider hpSlider;

    private HeroUnit Hero;

    public void Init(HeroUnit hero)
    {
        Hero = hero;
        hpSlider.maxValue = Hero.HPResource.MaxHP;
        hpSlider.value = Hero.HPResource.GetHP();
        Hero.OnHpChanged += UpdateHp;
    }

    private void UpdateHp(int newHp) => hpSlider.value = newHp;

}