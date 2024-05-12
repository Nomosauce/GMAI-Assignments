using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefBotFSM : MonoBehaviour
{
    ChefBaseState currentState;

    //variables for states
    public bool questDone;
    public int waterLeft;
    public int ingredientsLeft;
    public float stressMeter;
    public float maxStress;

    //bool flags for any state states
    public bool refuelFlag = false;
    public bool fatigueFlag = false;
    public bool preventAnyStateFlag = false;

    public ChefIdleState idleState;
    public ChefApproachState approachState;
    public ChefInquireState inquireState;
    public ChefQuestState questState;
    public ChefPrepareFoodState prepareFoodState;
    public ChefPrepareDrinkState prepareDrinkState;
    public ChefCollectWaterState collectWaterState;
    public ChefHarvestIngredientsState harvestIngredientsState;
    public ChefThrowState throwState;

    public ChefRefuelState refuelState;
    public ChefFatigueState fatigueState;

    // Start is called before the first frame update
    void Start()
    {
        questDone = false;
        waterLeft = 3;
        ingredientsLeft = 3;
        stressMeter = 0;
        maxStress = 10;

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

        refuelState = new ChefRefuelState(this);
        fatigueState = new ChefFatigueState(this);

        currentState = idleState; //set idle as current state
        currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fatigueFlag && stressMeter == maxStress) //transition condition (any state) - if it is not already in the fatigue state, and the stress meter is at maximum
        {
            fatigueFlag = true;
            SwitchState(fatigueState);
        }

        if (!refuelFlag && refuelNeeded() && !preventAnyStateFlag) //transition condition (any state) - if its not already in refuel state, refuel is needed and not in inquire state go to refuel state
        {
            refuelFlag = true;
            SwitchState(refuelState);
        }

        currentState.OnUpdate();
    }

    //transitioning to another state - function is used in the subclasses with a parameter (new state) passed in
    public void SwitchState(ChefBaseState newState)  
    {
        
        currentState.OnExit();
        currentState = newState; 
        currentState.OnEnter();
    }

    //functions to call the coroutines in the states as coroutine cannot start unless in monobehaviour
    public void RefillWaterCoroutine()
    {
        StartCoroutine(collectWaterState.refillingWater());
    }

    public void RefillIngredientsCoroutine()
    {
        StartCoroutine(harvestIngredientsState.refillingIngredients());
    }

    public void faintedCoroutine()
    {
        StartCoroutine(fatigueState.fainted());
    }

    public void addToStress(float stress) //certain states have a chance to add to the stress meter
    {
        if (stressChance() && stressMeter < maxStress)
        {
            stressMeter += stress; //stress chance is true when the random number is 5 and above, and when stress meter is max stress, add to the stress meter

            if (stressMeter > maxStress)
            {
                stressMeter = maxStress; //makes sure stress meter is capped to the max if it gets above the maximum
            }
        }
    }

    private bool stressChance()
    {
        if (Random.Range(0, 10) >= 5f) //random no. is picked from 0-10, if its 5 or more, returns true
        {
            return true;
        }
        return false;
    }

    private bool refuelNeeded()
    {
        if (Random.Range(0, 10) >= 8f) //random no. is picked from 0-10, if its 5 or more, returns true
        {
            return true;
        }
        return false;
    }
}
