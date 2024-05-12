using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefFatigueState : ChefBaseState
{
    //constructor pass reference to fsm when state is instantiated so fsm(variable) wont be null whenever states use fsm variables
    public ChefFatigueState(ChefBotFSM _fsm)
    {
        fsm = _fsm;
    }

    //methods inherited from the abstract class are overridden to give the state functionality
    public override void OnEnter()
    {
        fsm.preventAnyStateFlag = true;
        Debug.Log("FATIGUE: Fainted. Resting for 10 seconds");
        fsm.faintedCoroutine();
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        fsm.fatigueFlag = false; //fatigue flag boolean becomes false so it can be entered again
        fsm.preventAnyStateFlag = false;
    }

    public IEnumerator fainted()
    {
        yield return new WaitForSeconds(10f);
        fsm.stressMeter = 0; //reset the stress meter
        fsm.SwitchState(fsm.idleState); //transition to idle after 10 seconds
    }
}
