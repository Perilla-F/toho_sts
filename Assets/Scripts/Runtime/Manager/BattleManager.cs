using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private BattleCommandExecutor _executor;
    [SerializeField] private Button _turnEndButton;
    private BattleSystem _system;
    private IHeroUnit _hero;
    private EnemyManager _enemies;
    private TimelineManager _timelineManager;
    private IBattleContext _context;

    private int TurnCount;
    private BattlePhase _phase;
    private bool BattleFinished;

    private UniTaskCompletionSource<bool> _turnEndSource;

    public void Initialize(BattleSystem system, HeroUnit hero, EnemyManager enemy, TimelineManager timelineManager, BattleContext context)
    {
        _system = system;
        _hero = hero;
        _enemies = enemy;
        _timelineManager = timelineManager;
        _context = context;
    }

    public void BattleStart(CancellationToken ct)
    {
        try
        {
            UnityEngine.Debug.Log("Battle Start!");
            BattleFinished = false;
            TurnCount = 1;
            _context.Deck.Shuffle();
            BattleEventBus.Battle.OnBattleStart?.Invoke(_context, _hero.Mana);
            BattleFlowAsync(ct).Forget();
        }
        catch (OperationCanceledException)
        {
            // キャンセルされた時の後処理
            Debug.Log("バトルが中断されました。");
        }
    }

    public async UniTaskVoid BattleFlowAsync(CancellationToken ct)
    {
        while (!BattleFinished)
        {
            await ExecuteTurnStartFlowAsync(ct);
            if (BattleFinished) break;

            await ExecutePlayerSelectFlowAsync(ct);
            if (BattleFinished) break;

            await ExecuteTurnEndFlowAsync(ct);
            TurnCount++;
        }
    }

    /// <summary>
    /// ターン開始処理
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ExecuteTurnStartFlowAsync(CancellationToken ct)
    {
        Debug.Log("=== Turn Start Phase ===");
        _phase = BattlePhase.TurnStart;

        _timelineManager.RestTime();
        BattleEventBus.Turn.OnTurnStart?.Invoke(TurnCount, ct);
        _hero.Mana.RefleshMana();
        _hero.ProcessTurnStart();
        foreach (var enemy in _enemies.GetAllEnemies())
        {
            enemy.ProcessTurnStart();
        }

        _system.AddActionToTimeline();
        await _system.DrawMultipleAsync(_hero.DrawCount);
    }

    /// <summary>
    /// プレイヤー行動選択中
    /// </summary>
    /// <returns></returns>
    public async UniTask ExecutePlayerSelectFlowAsync(CancellationToken ct)
    {
        Debug.Log("=== Player Select Phase ===");
        _phase = BattlePhase.PlayerSelect;

        _turnEndButton.interactable = true;
        // ターン終了が起きるまで待つ
        _turnEndSource = new UniTaskCompletionSource<bool>();
        await _turnEndSource.Task;

        // ターン終了ボタン押下後
        // タイムラインに残ったすべてのイベント（主に敵の行動など）を消化
        while (_timelineManager.HasEvents())
        {
            // 次のイベントの時刻まで進めて実行
            await _timelineManager.ExecuteNextEventAsync(_context);
        }
    }

    /// <summary>
    /// ターン終了処理
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ExecuteTurnEndFlowAsync(CancellationToken ct)
    {
        Debug.Log("=== Turn End Phase ===");
        _phase = BattlePhase.TurnEnd;

        _hero.ProcessTurnEnd();
        foreach (var enemy in _enemies.GetAllEnemies())
        {
            enemy.ProcessTurnEnd();
        }

        await _executor.DiscardAllHandAsync(ct);
    }

    public void OnPushTurnEndButton()
    {
        if (_phase != BattlePhase.PlayerSelect) return;

        _turnEndButton.interactable = false;
        _turnEndSource?.TrySetResult(true);
    }

}
