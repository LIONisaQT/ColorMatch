using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State state)
    {
        this.state = state;
        StartCoroutine(state.Start());
    }
}
