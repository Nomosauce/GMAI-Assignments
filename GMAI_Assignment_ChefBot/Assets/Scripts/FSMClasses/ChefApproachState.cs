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
    }

    public override void OnUpdate()
    {
        if (playerLeft()) //transitions back to the idle state when this is true
        {
            fsm.SwitchState(fsm.idleState);
        }

        if (playerInteracts()) //transitions to the inquire state when this is true
        {
            fsm.SwitchState(fsm.inquireState);
        }
    }

    public override void OnExit()
    { }

    private bool playerLeft() //returns either true or false
    {
        if (Random.Range(0, 10) >= 5f) //random no. is picked from 0-10, if its 8 or more, returns true
        {
            return true;
        }
        return false;
    }

    private bool playerInteracts()
    {
        if (Random.Range(0, 10) >= 5f) //random no. is picked from 0-10, if its 2 or more, returns true
        {
            return true;
        }
        return false;
    }
}
