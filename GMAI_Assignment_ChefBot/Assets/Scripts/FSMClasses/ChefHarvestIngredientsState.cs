using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefHarvestIngredientsState : ChefBaseState
{
    private bool ingredientsRefilled; //coroutine flag bool

    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefHarvestIngredientsState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        fsm.preventAnyStateFlag = true;
        Debug.Log("HARVEST_INGREDIENTS: Harvest crops and animals");
        fsm.addToStress(0.5f);
        ingredientsRefilled = false;
    }

    public override void OnUpdate()
    {
        if (!ingredientsRefilled) //only start coroutine when its not already running...
        {
            fsm.RefillIngredientsCoroutine(); //function in fsm that calls the coroutine
        }
    }

    public override void OnExit()
    {
        fsm.preventAnyStateFlag = false;
    }

    public IEnumerator refillingIngredients()
    {
        ingredientsRefilled = true; //flag becomes true when coroutine enters
        yield return new WaitForSeconds(3f);
        fsm.ingredientsLeft = 3; //ingredients supply refilled, then transition back to prepare food state once coroutine done
        fsm.SwitchState(fsm.prepareFoodState);
    }
}
