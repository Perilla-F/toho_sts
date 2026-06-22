using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleEffectPlayer : MonoBehaviour
{
    private void OnEnable() => BattleEventBus.BattleEventAsync.OnEnemyAttackEffect += PlayEffect;

    private async UniTask DoProvisionalAnimation(IReadOnlyBattleUnit self)
    {
        await self.Model.PlayAttackAnimation();
    }

    private void PlayEffect(IReadOnlyBattleUnit self, UniTaskCompletionSource tcs)
    {
        // ここで暫定的なTween処理を行う
        // 最後に tcs.TrySetResult(); を呼ぶと、Executeの待機が解除される
        DoProvisionalAnimation(self).Forget();
        tcs.TrySetResult();
    }

    private void OnDestroy()
    {
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect -= PlayEffect;

    }
}