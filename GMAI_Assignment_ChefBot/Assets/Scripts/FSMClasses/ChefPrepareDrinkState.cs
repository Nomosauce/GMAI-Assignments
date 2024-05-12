using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefPrepareDrinkState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefPrepareDrinkState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("PREPARE_DRINK: Make drink");
        fsm.addToStress(0.2f);

        if (fsm.waterLeft > 0) //only deduct from water supply when the supply is above 0
        {
            fsm.waterLeft -= 1;
            Debug.Log($"Water left: {fsm.waterLeft}");
        }
        else //transition condition - at 0 (or below), go collect water state for refill
        {
            Debug.Log("No more water!");
            fsm.addToStress(0.2f);
            fsm.SwitchState(fsm.collectWaterState);
        }
    }

    public override void OnUpdate()
    {
        if (drinkDone()) //transition condition - go back to approach state once drink is done
        {
            Debug.Log("Drink is ready");
            fsm.SwitchState(fsm.approachState);
        }
    }

    public override void OnExit()
    { }

    private bool drinkDone()
    {
        if (Random.Range(0, 10) >= 2f) //random no. is picked from 0-10, if its 2 or more, returns true
        {
            return true;
        }
        return false;
    }
}
