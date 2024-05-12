using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefRefuelState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefRefuelState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("REFUEL: Relight the fire that has extinguished.");
        fsm.addToStress(0.2f);
        fsm.SwitchState(fsm.idleState); //transition condition - go back to idle after refueling done
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        fsm.refuelFlag = false; //boolean flag becomes false once state is exited so it can be entered again
    }
}
