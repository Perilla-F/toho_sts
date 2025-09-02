using System.Collections.Generic;

public class StateManager
{
    private Dictionary<BattleStateType, IBattleState> _states = new();
    private IBattleState _currentState;

    public void RegisterState(BattleStateType type, IBattleState state)
    {
        _states[type] = state;
    }

    public void ChangeState(BattleStateType type)
    {
        _currentState?.OnExit();
        _currentState = _states[type];
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}
