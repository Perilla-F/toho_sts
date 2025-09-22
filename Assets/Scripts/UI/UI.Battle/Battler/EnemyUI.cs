using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;

    public void Init(EnemyUnit enemy)
    {
        hpSlider.maxValue = enemy.MaxHP;
        hpSlider.value = enemy.CurrentHP;

        enemy.OnHpChanged += UpdateHp;
    }

    void UpdateHp(int newHp)
    {
        hpSlider.value = newHp;
    }
}