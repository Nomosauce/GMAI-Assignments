using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefPrepareFoodState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefPrepareFoodState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        Debug.Log("PREPARE_FOOD: Take ingredients from kitchen and cook them into food");
        fsm.addToStress(0.2f);

        if (fsm.ingredientsLeft > 0) //only deduct from ingredients supply when the supply is above 0
        {
            fsm.ingredientsLeft -= 1;
            Debug.Log($"Ingredients left: {fsm.ingredientsLeft}");
        }
        else //transition condition - at 0 (or below), go harvest ingredient state for refill
        {
            Debug.Log("No more meat/crops!");
            fsm.addToStress(0.2f);
            fsm.SwitchState(fsm.harvestIngredientsState);
        }
    }

    public override void OnUpdate()
    {
        if (!foodSpoiled()) //transition condition - go back to approach state once food is done (if food isnt messed up)
        {
            Debug.Log("Food is ready");
            fsm.SwitchState(fsm.approachState);
        }
        else
        {
            Debug.Log("Food got burnt/spoiled!");
            fsm.SwitchState(fsm.throwState); //transition condition - go throw state (if food is messed up)
        }
    }

    public override void OnExit()
    { }

    private bool foodSpoiled()
    {
        if (Random.Range(0, 10) >= 8) //random no. is picked from 0-10, if its 8 or more, returns true
        {
            return true;
        }
        return false;
    }
}
