using System.Collections;

public abstract class State
{
    protected StateMachine stateMachine;

    protected StateView stateView;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Pause()
    {
        yield break;
    }

    public virtual IEnumerator End()
    {
        yield break;
    }
}
