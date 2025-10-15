using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text durationText; // 残り秒数のテキスト（任意）

    private StatusEffect currentStatus;

    public void SetStatus(StatusEffect status)
    {
        currentStatus = status;
        iconImage.sprite = status.Data.icon;
        UpdateDisplay();
    }

    private void Update()
    {
        if (currentStatus == null) return;

        // 残り時間更新
        currentStatus.AddStacks(-1);

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (durationText != null)
        {
            durationText.text = currentStatus.Stacks.ToString();
        }
    }

    public bool IsExpired() => currentStatus != null && currentStatus.Stacks <= 0;
}