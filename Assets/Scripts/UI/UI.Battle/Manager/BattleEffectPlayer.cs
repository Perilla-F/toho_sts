using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleEffectPlayer : MonoBehaviour
{
    private IEnemyManager _enemyManager;
    private ModelRegistory _modelRegistory;

    private void Awake()
    {
        BattleEventBus.BattleEventAsync.OnCardUsed += HandleCardUsed;
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect += HandleEnemyAction;
    }

    public void Initialized(IEnemyManager enemyManager, ModelRegistory modelRegistory)
    {
        _enemyManager = enemyManager;
        _modelRegistory = modelRegistory;
    }

    private async UniTask DoProvisionalAnimation(IAnimatable animatable)
    {
        // これなら、ユニットの種類に関わらず「再生できるもの」を再生するだけ
        await animatable.PlayAttackAnimation();
    }

    private void HandleCardUsed(IReadOnlyCardContext context, UniTaskCompletionSource tcs)
    {
        var model = _modelRegistory.GetModelForUnit(context.User);
        PlayCardEffect(model, tcs).Forget();
    }

    private async UniTaskVoid PlayCardEffect(IAnimatable user, UniTaskCompletionSource tcs)
    {
        await DoProvisionalAnimation(user);
        tcs.TrySetResult();
    }

    private void HandleEnemyAction(int id, UniTaskCompletionSource tcs)
    {
        var self = _enemyManager.GetEnemy(id);
        var model = _modelRegistory.GetModelForUnit(self);
        PlayEnemyEffect(model, tcs).Forget();
    }

    private async UniTaskVoid PlayEnemyEffect(IAnimatable self, UniTaskCompletionSource tcs)
    {
        // ここで暫定的なTween処理を行う
        // 最後に tcs.TrySetResult(); を呼ぶと、Executeの待機が解除される
        await DoProvisionalAnimation(self);
        tcs.TrySetResult();
    }

    private void OnDestroy()
    {
        BattleEventBus.BattleEventAsync.OnCardUsed -= HandleCardUsed;
        BattleEventBus.BattleEventAsync.OnEnemyAttackEffect -= HandleEnemyAction;
    }
}