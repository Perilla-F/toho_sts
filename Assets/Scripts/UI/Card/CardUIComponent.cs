using System;
using UnityEngine;

public class CardUIComponent : MonoBehaviour
{
    [SerializeField] private CardVisual visual;

    // バトル外（デッキ確認画面など）で使う初期化
    public void SetupForDeckMenu(SourceCard source)
    {
        // バフなしの素の状態で見た目を更新
        visual.UpdateVisual(source);

        // ドラッグやクリックでプレイできないようにする無効化処理など
        SetInteractive(false);
    }

    // バトル中（山札・捨て札確認画面など）で使う初期化
    public void SetupForBattleViewer(IReadOnlyCardObj cardObj, IReadOnlyBattleContext battleContext)
    {
        // バトル中のバフ（強化など）を反映して見た目を更新
        // ※必要に応じてバトル中のコンテキストを渡す
        visual.UpdateVisual(cardObj, battleContext);

        SetInteractive(false);
    }

    private void SetInteractive(bool isInteractive)
    {
        // ドラッグ検知やボタン反応のON/OFFを切り替える
    }
}