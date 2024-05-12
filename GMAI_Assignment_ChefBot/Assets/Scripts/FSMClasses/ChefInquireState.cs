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
        fsm.addToStress(0.2f);
        fsm.preventAnyStateFlag = true; //prevent any states from transitioning while on this state

        if (!fsm.questDone) //if quest has not been done, quest option is available - once done, option is unavailable anymore
        {
            Debug.Log("INQUIRE: Wait for Player input \nPress 1 - FOOD \nPress 2 - DRINK \nPress 3 - QUEST \nPress SPACE - Cancel");
        }
        else
        {
            Debug.Log("INQUIRE: Wait for Player input \nPress 1 - FOOD \nPress 2 - DRINK \nPress SPACE - Cancel");
        }
    }

    public override void OnUpdate()
    {
        if (fsm.preventAnyStateFlag) //turns the boolean flag true so any states cannot interfere inquire state (so can actually input)
        {
            //transition to other states with players input being the condition
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("You picked FOOD");
                fsm.SwitchState(fsm.prepareFoodState);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("You picked DRINK");
                fsm.SwitchState(fsm.prepareDrinkState);
            }
            else if (!fsm.questDone && Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("You picked QUEST");
                fsm.SwitchState(fsm.questState);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Cancel");
                fsm.SwitchState(fsm.approachState);
            }
        }
    }

    public override void OnExit()
    {
        fsm.preventAnyStateFlag = false; //turns it to false again so any states can be transitioned into again
    }
}
