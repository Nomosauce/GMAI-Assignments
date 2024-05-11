using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefBotFSM : MonoBehaviour
{
    ChefBaseState currentState;

    public ChefIdleState idleState;
    public ChefApproachState approachState;
    public ChefInquireState inquireState;

    // Start is called before the first frame update
    void Start()
    {
        //create instances of the states and pass the fsm in as parameter
        idleState = new ChefIdleState(this);
        approachState = new ChefApproachState(this);
        inquireState = new ChefInquireState(this);

        currentState = idleState; //set idle as current state
        currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
    }

    //transitioning to another state - function is used in the subclasses with a parameter (new state) passed in
    public void SwitchState(ChefBaseState newState)  
    {
        currentState.OnExit(); 
        currentState = newState; 
        currentState.OnEnter();
    }
}
