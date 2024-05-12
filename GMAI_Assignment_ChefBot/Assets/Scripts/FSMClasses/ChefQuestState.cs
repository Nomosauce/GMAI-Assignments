using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefQuestState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefQuestState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        fsm.preventAnyStateFlag = true;
        Debug.Log("QUEST: Follow Player until quest is done");
    }

    public override void OnUpdate()
    {
        if (finishQuest()) //transition condition - go back to idle state once quest finish
        {
            Debug.Log("Quest is done");
            fsm.questDone = true; //set the quest done bool to true so the condition is met
            fsm.SwitchState(fsm.idleState);
        }
    }

    public override void OnExit()
    {
        fsm.preventAnyStateFlag = false;
    }

    private bool finishQuest()
    {
        if (Random.Range(0, 10) >= 5f) //random no. is picked from 0-10, if its 5 or more, returns true
        {
            return true;
        }
        return false;
    }
}
