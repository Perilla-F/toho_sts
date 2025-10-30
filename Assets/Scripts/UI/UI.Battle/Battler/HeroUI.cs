using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour, IBattleUI
{
    [SerializeField] private Slider hpSlider;

    private IHeroUnit Hero;

    public void Init(IHeroUnit hero)
    {
        Hero = hero;
        hpSlider.maxValue = Hero.HPResource.MaxHP;
        hpSlider.value = Hero.HPResource.GetHP();
    }

    public void UpdateHp(int newHp) => hpSlider.value = newHp;

}