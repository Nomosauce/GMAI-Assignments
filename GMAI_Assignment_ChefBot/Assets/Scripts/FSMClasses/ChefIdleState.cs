using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefIdleState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefIdleState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("IDLE: Rest/Do nothing");
        fsm.addToStress(0.1f);
    }

    public override void OnUpdate()
    {
        if (playerIsNear()) //transition condition - go to the approach state when this is true
        {
            fsm.SwitchState(fsm.approachState);
        }
    }

    public override void OnExit()
    { }

    private bool playerIsNear() //returns either true or false
    {
        if (Random.Range(0, 10) >= 2f) //random no. is picked from 0-10, if its 2 or more, returns true
        {
            return true;
        }
        return false;
    }
}
