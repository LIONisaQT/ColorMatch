using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeState : State
{
    public HomeState(StateMachine stateMachine) : base(stateMachine)
    {
        stateView = new HomeStateView();
    }

    public override IEnumerator Start()
    {
        Debug.Log("HomeState Start!");

        yield return new WaitForSeconds(0);
    }
}
