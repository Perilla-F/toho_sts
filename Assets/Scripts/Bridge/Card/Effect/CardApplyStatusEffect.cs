using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "CardEffect/Status")]
public class CardApplyStatusEffect : CardEffectDefinition
{
    public EffectData Effect;
    public override string KeywordDetail
    {
        get
        {
            // 1. 状態異常データがセットされていれば、そのデータの説明文を優先して返す
            if (Effect != null && !string.IsNullOrEmpty(Effect.Description))
            {
                return Effect.Description;
            }

            // 2. もしデータがセットされていない（または空の）場合は、親クラスの手動入力をフォールバックとして使う
            return base.KeywordDetail;
        }
    }

    public override void Execute(ICardContext context, IBattleUnit target, int amount)
    {
        target.AddEffect(Effect, amount);
        BattleEventBus.View.OnUpdateHp(target);
    }
}