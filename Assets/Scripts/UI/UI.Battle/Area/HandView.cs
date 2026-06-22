using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class HandView : MonoBehaviour, IHandView
{
    [SerializeField] private float radius;

    private List<BattleCard> _cards = new List<BattleCard>();

    public Transform GetTransform() => transform;

    /// <summary>
    /// 手札を整列させる
    /// </summary>
    public async UniTask ArrangeCards(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        float angleStep = Mathf.Min(5f, 60f / _cards.Count);
        float totalAngle = angleStep * (_cards.Count - 1);
        float startAngle = totalAngle / 2f;

        var tasks = new List<UniTask>();

        for (int i = 0; i < _cards.Count; i++)
        {
            float currentAngle = (i * angleStep) - startAngle;
            float x = -Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius - radius;

            Vector3 pos = new Vector3(x, y, 0);
            int sibling = _cards.Count - i - 1;

            tasks.Add(_cards[i].SetLayoutPosition(pos, currentAngle, sibling));
        }

        await UniTask.WhenAll(tasks);

        BattleEventBus.Card.RestoreAllCards?.Invoke();
    }

    public void AddCard(BattleCard card)
    {
        _cards.Add(card);
    }

    public void Discard(BattleCard card)
    {
        _cards.Remove(card);
    }

    public List<BattleCard> GetCards()
    {
        return _cards;
    }
}
