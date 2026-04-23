using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class HandUIManager : ICardUIHandler
{
    private BattleViewRoot view;

    private ICardPoolProvider pool;

    public HandUIManager(BattleViewRoot view)
    {
        this.view = view;
    }

    public void Setup(ICardPoolProvider pool)
    {
        this.pool = pool;
    }

    public async UniTask PlayDrawAnimationAsync(DrawEventData data, CancellationToken ct)
    {
        var card = pool.GetCard();

        card.BindCard(data.cardObj);

        // card(IPoolableCard)をBattleCardにキャスト
        if (card is MonoBehaviour mono)
        {
            mono.transform.position = view.DeckView.GetTransform().position;

            // 手札の目標座標を計算
            Vector3 zero = Vector3.zero;
            // Vector3 targetPos = CalculatePos(data.drawIndex);

            // DOTweenをUniTaskに変換して、移動が終わるまでここで待機する
            await mono.transform.DOMove(zero, 0.5f)
                .SetEase(Ease.OutCubic)
                .ToUniTask(cancellationToken: ct);

        }

        // 移動が終わった後にさらに何か演出を入れることも可能
        Debug.Log($"{data.cardObj.Source.Data.CardName} の移動完了！");
    }

    public async UniTask PlayDiscardAnimationAsync(DiscardEventData data, CancellationToken ct) { }
}