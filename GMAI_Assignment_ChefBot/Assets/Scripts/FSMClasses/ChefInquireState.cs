using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefInquireState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefInquireState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("INQUIRE: Wait for Player input \nPress 1 - Quest \nPress 2 - Food \nPress 3 - Drink \nPress 4 - Cancel");
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("You picked QUEST");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("You picked FOOD");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("You picked DRINK");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("CANCEL");
            fsm.SwitchState(fsm.approachState);
        }
    }

    public override void OnExit()
    { }
}
