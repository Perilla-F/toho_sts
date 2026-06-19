using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconSpace;
    [SerializeField] private TextMeshProUGUI _stacks;

    // ロードしたハンドルを保持（後でメモリを解放するため）
    private AsyncOperationHandle<Sprite> _handle;
    public StatusEffect effect;

    public void SetIcon(StatusEffect effect)
    {
        this.effect = effect;
        _stacks.text = effect.Stacks.ToString();

        // 以前のロードがあれば解放してメモリを空ける
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }

        // 非同期でロード開始
        _handle = Addressables.LoadAssetAsync<Sprite>(effect.Data.IconRef);
        _handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                _iconSpace.sprite = op.Result;
            }
        };
    }

    public void UpdateIcon(StatusEffect effect)
    {
        _stacks.text = effect.Stacks.ToString();
    }

    private void OnDestroy()
    {
        // オブジェクトが消える時にメモリを解放（重要！）
        if (_handle.IsValid())
        {
            Addressables.Release(_handle);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string title = effect.Data.DisplayName;
        string desc = effect.Data.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}