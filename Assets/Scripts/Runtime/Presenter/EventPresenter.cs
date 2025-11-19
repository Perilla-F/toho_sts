public class EventPresenter
{
    private readonly EventManager _eventManager;
    private readonly EventUIManager _eventUIManager;
    private readonly RestUIManager _restUIManager;

    public EventPresenter(EventManager manager, EventUIManager eventUI, RestUIManager restUI)
    {
        _eventManager = manager;
        _eventUIManager = eventUI;
        _restUIManager = restUI;

        // イベントを購読
        _eventManager.OnStepStarted += HandleStepStarted;
        _eventManager.OnEventEnded += HandleEventEnded;

        // UIからの入力も購読
        _eventUIManager.OnOptionSelected += HandleOptionSelected;
        _restUIManager.OnRestPush += HandleRestPush;
    }

    private void HandleStepStarted(EventStep step)
    {
        _eventUIManager.ShowStep(step);
    }

    private void HandleEventEnded()
    {
        _eventUIManager.Hide();
    }

    private void HandleOptionSelected(EventOption option)
    {
        _eventManager.SelectOption(option);
    }

    private void HandleRestPush()
    {
        _eventManager.StartRestEvent();
    }
}
