using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

public class CardVisual : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI delayText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Rarity Frames")]
    [SerializeField] private Sprite commonFrame;
    [SerializeField] private Sprite uncommonFrame;
    [SerializeField] private Sprite rareFrame;

    private AsyncOperationHandle<Sprite> _handle;
    private string _lastLoadedKey = string.Empty;
    private CancellationTokenSource _cts;
    private static Dictionary<string, AsyncOperationHandle<Sprite>> _sharedHandles = new();
    private static HashSet<string> _loadingKeys = new();

    /// <summary>
    /// 外部からCardDataを設定し、UIを更新する
    /// </summary>
    public void UpdateVisual(CardData data)
    {
        if (data == null)
        {
            Debug.LogWarning("CardData is null!");
            return;
        }
        // 1. テキスト情報の更新
        nameText.text = data.CardName;
        descriptionText.text = data.Description;

        // 2. レアリティに応じたフレームの切り替え
        SetFrame(data.Rarity);
        SetCost(data.Costs);

        delayText.text = data.Delay.ToString();

        LoadIllustrationAsync(data.CardIllustration).Forget();
    }

    private void SetFrame(CardRarity rarity)
    {
        frameImage.sprite = rarity switch
        {
            CardRarity.Common => commonFrame,
            CardRarity.Uncommon => uncommonFrame,
            CardRarity.Rare => rareFrame,
            _ => commonFrame
        };
    }

    private void SetCost(List<ResourceCost> costs)
    {
        if (costs != null && costs.Count > 0)
        {
            int cost = 0;
            foreach (var c in costs)
            {
                if (c.Type == ResourceType.Mana)
                {
                    cost = c.Amount;
                }
            }
            costText.text = cost.ToString();
        }
        else
        {
            costText.text = "-";
        }
    }

    private async UniTaskVoid LoadIllustrationAsync(AssetReferenceSprite reference, CancellationToken ct = default)
    {
        if (reference == null || !reference.RuntimeKeyIsValid())
        {
            artworkImage.sprite = null;
            return;
        }

        string newKey = reference.RuntimeKey.ToString();

        // 1. すでに誰かがロード完了している場合
        if (_sharedHandles.TryGetValue(newKey, out var existingHandle) && existingHandle.Status == AsyncOperationStatus.Succeeded)
        {
            artworkImage.sprite = existingHandle.Result;
            _lastLoadedKey = newKey; // インスタンスごとの記録も更新
            return;
        }

        // 2. 今まさに誰かがロード中の場合
        if (_loadingKeys.Contains(newKey))
        {
            await UniTask.WaitUntil(() => !_loadingKeys.Contains(newKey),
                                            PlayerLoopTiming.Update, // タイミングを明示
                                            cancellationToken: ct);
            if (_sharedHandles.TryGetValue(newKey, out var finishedHandle))
            {
                artworkImage.sprite = finishedHandle.Result;
                _lastLoadedKey = newKey;
            }
            return;
        }

        // 3. 誰もロードしていない場合（自分が代表してロードする）
        _loadingKeys.Add(newKey);
        _lastLoadedKey = newKey;

        // 前のロードがあれば解放（自分自身の古いハンドルがあれば）
        // ※static管理にする場合、Releaseタイミングが複雑になるため、
        // シンプルにするなら「一度ロードした画像はバトル中保持する」のが安全です

        var handle = reference.LoadAssetAsync();
        _sharedHandles[newKey] = handle;

        try
        {
            await handle.ToUniTask(cancellationToken: ct);
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                artworkImage.sprite = handle.Result;
            }
        }
        finally
        {
            _loadingKeys.Remove(newKey);
        }
    }

    public void Cleanup()
    {
        _cts?.Cancel();
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
            _handle = default;
        }
        _lastLoadedKey = string.Empty;
        artworkImage.sprite = null;
    }

    public static void ReleaseAll()
    {
        foreach (var h in _sharedHandles.Values)
        {
            if (h.IsValid()) Addressables.Release(h);
        }
        _sharedHandles.Clear();
        _loadingKeys.Clear();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

}
