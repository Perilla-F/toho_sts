using UnityEngine;

public interface IPoolableCard
{
    GameObject GameObject { get; }

    void BindCard(ICardObj obj);

    // プールから取り出された時の初期化（データの注入）
    void SetupCardData(CardData data);

    // プールに戻る直前の掃除（Addressablesの解放など）
    void ResetForPool();
}