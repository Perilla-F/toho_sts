using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HeroData")]
public class HeroData : ScriptableObject
{
    [SerializeField] private String _battlerName;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _maxMana;
    [SerializeField] public GameObject Live2DModelPrefab { get; set; }
    [SerializeField] private Sprite _portrait;
    [SerializeField] public List<CardData> StartingDeck;
    [SerializeField] public GameObject UIPrefab;    // HPバーなどのUIPrefab
    [SerializeField] public GameObject ModelPrefab;
    public float ModelYOffset;
    [SerializeField] public AnimationClip IdleClip;
    [SerializeField] public AnimationClip AttackClip;
    [SerializeField] public AnimationClip HitClip;


    public String BattlerName => _battlerName;
    public int MaxHP => _maxHP;
    public Sprite Portrait => _portrait;
    public RuntimeAnimatorController AnimatorController { get; }

}
