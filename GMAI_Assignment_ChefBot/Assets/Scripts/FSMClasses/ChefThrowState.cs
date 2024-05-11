using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefThrowState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefThrowState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("THROW: Throw burnt/spoiled food away");
    }

    public override void OnUpdate()
    {
        if (foodThrown())
        {
            fsm.SwitchState(fsm.prepareFoodState);
        }
    }

    public override void OnExit()
    { }

    private bool foodThrown()
    {
        if (Random.Range(0, 10) >= 2f) //random no. is picked from 0-10, if its 2 or more, returns true
        {
            return true;
        }
        return false;
    }
}
