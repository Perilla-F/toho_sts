using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HeroData")]
public class HeroData : ScriptableObject, IBattleHeroData
{
    [SerializeField] private String _battlerName;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _maxMana;
    [SerializeField] public GameObject Live2DModelPrefab { get; set; }
    [SerializeField] private Sprite _portrait;
    [SerializeField] public List<CardData> StartingDeck;
    [SerializeField] public AnimationClip IdleMotionClip { get; set; }
    // [SerializeField] private string attackAnimationName;
    public String BattlerName => _battlerName;
    public int MaxHP => _maxHP;
    public Sprite Portrait => _portrait;
    public RuntimeAnimatorController AnimatorController { get; }
    // public string AttackAnimation => attackAnimationName; public String BattlerName => battlerName;

}
