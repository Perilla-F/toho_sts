using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour, IBattleUI
{
    [SerializeField] Slider hpSlider;

    public void Init(EnemyUnit enemy)
    {
        hpSlider.maxValue = enemy.HPResource.MaxHP;
        hpSlider.value = enemy.HPResource.GetHP();

        enemy.OnHpChanged += UpdateHp;
    }

    void UpdateHp(int newHp)
    {
        hpSlider.value = newHp;
    }
}