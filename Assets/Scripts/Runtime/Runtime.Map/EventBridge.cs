using UnityEngine;

public class EventBridge : MonoBehaviour
{
    [SerializeField] private EventUIManager uiManager;

    private void Awake()
    {
        EventRunner.Instance.OnStepChanged += HandleStepChanged;
        EventRunner.Instance.OnEventEnded += HandleEventEnded;

        uiManager.OnOptionSelected += HandleOptionSelected;
    }

    private void HandleStepChanged(EventStep step)
    {
        uiManager.ShowStep(step);
    }

    private void HandleOptionSelected(EventOption option)
    {
        EventRunner.Instance.SelectOption(option);
    }

    private void HandleEventEnded()
    {
        uiManager.Hide();
    }

    private void OnDestroy()
    {
        if (EventRunner.Instance == null) return;

        EventRunner.Instance.OnStepChanged -= HandleStepChanged;
        EventRunner.Instance.OnEventEnded -= HandleEventEnded;
        uiManager.OnOptionSelected -= HandleOptionSelected;
    }
}