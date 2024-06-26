using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefApproachState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefApproachState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("APPROACH: Go to the counter");
        fsm.addToStress(0.2f);
    }

    public override void OnUpdate()
    {
        if (Random.Range(0,10) >= 8) //transition condition - go back to the idle state if the random no. (0-10) is 8 or above - if below 8, go to inquire state
        {
            fsm.SwitchState(fsm.idleState);
        }
        else
        {
            fsm.SwitchState(fsm.inquireState);
        }
    }

    public override void OnExit()
    { }
}
