using System.Collections.Generic;

public class StateManager
{
    private Dictionary<BattleStateType, IBattleState> states = new();
    private IBattleState currentState;

    public void RegisterState(BattleStateType type, IBattleState state)
    {
        states[type] = state;
    }

    public void ChangeState(BattleStateType type)
    {
        currentState?.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
