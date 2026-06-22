using UnityEngine;
using UnityEngine.Pool;

public class CardPoolManager : ICardPoolProvider
{
    private GameObject cardPrefab;
    private IObjectPool<IPoolableCard> _pool;

    public CardPoolManager(GameObject card)
    {
        cardPrefab = card;

        Setup();
    }

    private void Setup()
    {
        _pool = new ObjectPool<IPoolableCard>(
            createFunc: () =>
            {
                var obj = Object.Instantiate(cardPrefab);
                // 生成したプレハブにインターフェースがついていることだけを確認
                return obj.GetComponent<IPoolableCard>();
            },
            actionOnGet: card => card.GameObject.SetActive(true),
            actionOnRelease: card =>
            {
                card.ResetForPool();
                card.GameObject.SetActive(false);
            }
        );
    }

    public IPoolableCard GetCard() => _pool.Get();
    public void ReturnCard(IPoolableCard card) => _pool.Release(card);

}