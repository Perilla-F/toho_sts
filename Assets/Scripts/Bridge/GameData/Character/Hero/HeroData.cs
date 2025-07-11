using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HeroData")]
public class HeroData : ScriptableObject, IBattleHeroData
{
    [SerializeField] private String battlerName;
    [SerializeField] private int maxHP;
    [SerializeField] private int maxMana;
    [SerializeField] public GameObject Live2DModelPrefab { get; set; }
    [SerializeField] private Sprite portrait;
    [SerializeField] public List<CardData> startingDeck;
    [SerializeField] public AnimationClip IdleMotionClip { get; set; }
    // [SerializeField] private string attackAnimationName;
    public String BattlerName => battlerName;
    public int MaxHP => maxHP;
    public int Attack => attack;
    public Sprite Portrait => portrait;
    public RuntimeAnimatorController AnimatorController { get; }
    // public string AttackAnimation => attackAnimationName; public String BattlerName => battlerName;

}
