using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EnemyViewer : MonoBehaviour, IDropHandler
{

    [SerializeField] Animator Animator;
    [SerializeField] private Slider _hpSlider;

    // public void Setup(IBattlerBaseData data)
    // {
    //     Animator.runtimeAnimatorController = data.AnimatorController;
    // }

    public void UpdateHP(int current, int max)
    {
        _hpSlider.value = (float)current / max;
    }

    public void PlayAnimation(string animationName)
    {
        Animator.Play(animationName);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("drop!");
        CardObj card = eventData.pointerDrag.GetComponent<CardObj>();
    }

}
