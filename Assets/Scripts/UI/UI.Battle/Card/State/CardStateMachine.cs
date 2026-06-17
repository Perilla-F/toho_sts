public class CardStateMachine
{
    public CardStateBase CurrentState { get; private set; }
    private BattleCard _owner;

    public CardStateMachine(BattleCard owner)
    {
        _owner = owner;
    }

    public void ChangeState(CardStateBase newState)
    {
        // 現在の状態に終了を通知
        CurrentState?.OnExit();

        CurrentState = newState;

        // 新しい状態に開始を通知
        CurrentState?.OnEnter();
    }

    public void Update()
    {
        CurrentState?.OnUpdate();
    }
}