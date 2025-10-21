using UnityEngine;

public class EventBridge : MonoBehaviour
{
    [SerializeField] private EventUIManager uiManager;
    private EventRunner runner;

    private void Awake()
    {
        runner.OnStepChanged += HandleStepChanged;
        runner.OnEventEnded += HandleEventEnded;

        uiManager.OnOptionSelected += HandleOptionSelected;
    }

    private void HandleStepChanged(EventStep step)
    {
        uiManager.ShowStep(step);
    }

    private void HandleOptionSelected(EventOption option)
    {
        runner.SelectOption(option);
    }

    private void HandleEventEnded()
    {
        uiManager.Hide();
    }

    private void OnDestroy()
    {
        if (runner == null) return;

        runner.OnStepChanged -= HandleStepChanged;
        runner.OnEventEnded -= HandleEventEnded;
        uiManager.OnOptionSelected -= HandleOptionSelected;
    }
}