public class EventPresenter
{
    private readonly EventManager _eventManager;
    private readonly EventUIManager _uiManager;

    public EventPresenter(EventManager manager, EventUIManager panel)
    {
        _eventManager = manager;
        _uiManager = panel;

        // イベントを購読
        _eventManager.OnStepStarted += HandleStepStarted;
        _eventManager.OnEventEnded += HandleEventEnded;

        // UIからの入力も購読
        _uiManager.OnOptionSelected += HandleOptionSelected;
    }

    private void HandleStepStarted(EventStep step)
    {
        _uiManager.ShowStep(step);
    }

    private void HandleEventEnded()
    {
        _uiManager.Hide();
    }

    private void HandleOptionSelected(EventOption option)
    {
        _eventManager.SelectOption(option);
    }
}
