using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChefBotFSM : MonoBehaviour
{
    ChefBaseState currentState;

    //variables for states
    public bool questDone = false;
    public int waterLeft = 3;
    public int ingredientsLeft = 3;

    public ChefIdleState idleState;
    public ChefApproachState approachState;
    public ChefInquireState inquireState;
    public ChefQuestState questState;
    public ChefPrepareFoodState prepareFoodState;
    public ChefPrepareDrinkState prepareDrinkState;
    public ChefCollectWaterState collectWaterState;
    public ChefHarvestIngredientsState harvestIngredientsState;
    public ChefThrowState throwState;

    // Start is called before the first frame update
    void Start()
    {
        //create instances of the states and pass the fsm in as parameter
        idleState = new ChefIdleState(this);
        approachState = new ChefApproachState(this);
        inquireState = new ChefInquireState(this);
        questState = new ChefQuestState(this);
        prepareFoodState = new ChefPrepareFoodState(this);
        prepareDrinkState = new ChefPrepareDrinkState(this);
        collectWaterState = new ChefCollectWaterState(this);
        harvestIngredientsState = new ChefHarvestIngredientsState(this);
        throwState = new ChefThrowState(this);

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

    public void RefillWaterCoroutine()
    {
        StartCoroutine(collectWaterState.refillingWater());
    }

    public void RefillIngredientsCoroutine()
    {
        StartCoroutine(harvestIngredientsState.refillingIngredients());
    }
}
