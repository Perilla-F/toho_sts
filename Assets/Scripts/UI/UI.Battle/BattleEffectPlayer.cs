using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleEffectPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        BattleEventBus.BattleEventAsync.OnCardUsed += (CardData data, IReadOnlyCardContext context, UniTaskCompletionSource tcs) => PlayCardEffect(data, context, tcs).Forget();
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect += (IReadOnlyBattleUnit self, UniTaskCompletionSource tcs) => PlayEnemyEffect(self, tcs).Forget();
    }

    private async UniTask DoProvisionalAnimation(IReadOnlyBattleUnit self)
    {
        await self.Model.PlayAttackAnimation();
    }

    private async UniTaskVoid PlayCardEffect(CardData data, IReadOnlyCardContext context, UniTaskCompletionSource tcs)
    {
        await DoProvisionalAnimation(context.User);
        tcs.TrySetResult();
    }

    private async UniTaskVoid PlayEnemyEffect(IReadOnlyBattleUnit self, UniTaskCompletionSource tcs)
    {
        // ここで暫定的なTween処理を行う
        // 最後に tcs.TrySetResult(); を呼ぶと、Executeの待機が解除される
        await DoProvisionalAnimation(self);
        tcs.TrySetResult();
    }

    private void OnDestroy()
    {
        BattleEventBus.BattleEventAsync.OnCardUsed -= (CardData data, IReadOnlyCardContext context, UniTaskCompletionSource tcs) => PlayCardEffect(data, context, tcs).Forget();
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect -= (IReadOnlyBattleUnit self, UniTaskCompletionSource tcs) => PlayEnemyEffect(self, tcs).Forget();
    }
}