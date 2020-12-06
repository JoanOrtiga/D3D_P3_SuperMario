public class StateMachine<T>
{
    private T owner;

    private State<T> currentState;
    private State<T> previousState;

    public StateMachine(T _owner)
    {
        this.owner = _owner;
    }

    public void ChangeState(State<T> newState)
    {
        previousState = currentState;

        if (currentState != null)
        {
            currentState.Exit(owner);
        }

        currentState = newState;
        currentState.Enter(owner);
    }

    public void UpdateMachine()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    public State<T> CurrentState()
    {
        return currentState;
    }

    public State<T> PreviousState()
    {
        return previousState;
    }

    public bool IsInState(State<T> state)
    {
        return currentState == state;
    }
}

