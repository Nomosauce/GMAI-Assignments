using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefCollectWaterState : ChefBaseState
{
    private bool waterRefilled; //coroutine flag bool

    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefCollectWaterState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        fsm.preventAnyStateFlag = true;
        Debug.Log("COLLECT_WATER: Get more water from a well");
        fsm.addToStress(0.5f);
        waterRefilled = false;
    }

    public override void OnUpdate()
    {
        if (!waterRefilled) //only start coroutine when its not already running...
        {
            fsm.RefillWaterCoroutine(); //function in fsm that calls the coroutine
        }
    }

    public override void OnExit()
    {
        fsm.preventAnyStateFlag = false;
    }

    public IEnumerator refillingWater()
    {
        waterRefilled = true; //flag becomes true when coroutine enters
        yield return new WaitForSeconds(3f);
        fsm.waterLeft = 3; //water supply refilled, then transition back to prepare drink state once coroutine done
        fsm.SwitchState(fsm.prepareDrinkState);
    }
}
