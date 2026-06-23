using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class CardEffectDefinition : ScriptableObject
{
    public abstract void Execute(ICardContext context, IBattleUnit target, int amount);
}