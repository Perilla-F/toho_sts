using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class CardEffectDefinition : ScriptableObject
{
    public abstract UniTask Apply(int amount, CardContext context);
}