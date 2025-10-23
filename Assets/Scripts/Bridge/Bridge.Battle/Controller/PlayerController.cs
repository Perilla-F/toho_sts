using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private void Awake() => Instance = this;

    public int? PredictedActionTime { get; private set; } = null;
    public int? PreviewActionTime { get; private set; } = null;

    [SerializeField] private TimelineManager timeline;
    public bool HasChosenAction { get; private set; } = false;
    public bool TurnEndRequested { get; private set; } = false;
    public CardObj ChosenCard { get; private set; }

    /// <summary>
    /// 行動選択開始処理
    /// </summary>
    public void BeginSelection()
    {
        HasChosenAction = false;
        TurnEndRequested = false;
    }

    /// <summary>
    /// 行動確定時処理
    /// </summary>
    /// <param name="card"></param>
    public void SelectAction(CardObj card)
    {
        ChosenCard = card;
        HasChosenAction = true;
    }

    /// <summary>
    /// 行動確定時処理
    /// </summary>
    public void ConfirmAction()
    {
        HasChosenAction = false;
        ChosenCard = null;
    }

    /// <summary>
    /// ターン終了ボタン押下状態
    /// </summary>
    public void TurnEndButton()
    {
        TurnEndRequested = true;
    }

    /// <summary>
    /// カードにマウスオーバーでプレビュー設定
    /// </summary>
    /// <param name="delay">カードデータのDelay</param>
    public void SetPreviewDelay(int delay)
    {
        PreviewActionTime = timeline.CurrentTime + delay;
    }

    /// <summary>
    /// カードからマウスを離れた時にプレビューを消す
    /// </summary>
    public void ClearPreview()
    {
        PreviewActionTime = null;
    }

}
