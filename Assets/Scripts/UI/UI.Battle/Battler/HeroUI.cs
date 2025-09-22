using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    private HeroUnit Hero;

    public void Init(HeroUnit hero)
    {
        Hero = hero;
        hpSlider.maxValue = Hero.MaxHP;
        hpSlider.value = Hero.CurrentHP;
        Hero.OnHpChanged += UpdateHp;
    }

    private void UpdateHp(int newHp) => hpSlider.value = newHp;

}