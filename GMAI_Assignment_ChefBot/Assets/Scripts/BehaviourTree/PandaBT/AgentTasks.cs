using UnityEngine;
using Panda;
using System.Runtime.CompilerServices;
using UnityEngine.AI;
using TMPro;
using System.Drawing;
using System.Linq;

public class AgentTasks : MonoBehaviour
{
    [Header("Speed")]
    public float agentWalkSpeed = 6f;
    public float agentRunSpeed = 13f;

    [Header("References")]
    public NavMeshAgent agent;
    public GameObject interactTxtGO;
    public TMP_Text interactTxt;
    public GameObject chefsGold;
    public ChefController chef;
    public PlayerController player;

    //general
    [Task]
    bool IsNear(string tag1, string tag2) //task that checks if two objects are close to each other by getting the Transform (since only position is needed) component of the tagged objects
    {
        //-side note, admittedly i looked into the example A2 sample for this and changed it slightly to fit this context
        Transform obj1 = GameObject.FindGameObjectWithTag(tag1).transform;
        Transform obj2 = GameObject.FindGameObjectWithTag(tag2).transform;
        float distance = 2f;

        return Vector3.Distance(obj1.position, obj2.position) <= distance; //vectorn.Distance method return the difference between the positions of the gameobjects and compares them to the set distance 
    }

    [Task]
    void GoTo(string tag)
    {
        //array of tags that are meant to be unaffected by the go to interrupt due to them being used for the angry tree. i referred to: https://www.tutorialspoint.com/how-to-check-in-chash-whether-the-string-array-contains-a-particular-work-in-a-string-array
        //this is to simplify the if statement to avoid excessive/inefficient use of || || ||
        string[] unaffectedTags = {"Player", "Table", "Trash", "Stove"};
        if (chef.isAngry && !(unaffectedTags.Contains(tag))) ThisTask.Succeed();//abort going to a destination if its angry (interruption to go to)

        Transform target = GameObject.FindGameObjectWithTag(tag).transform;
        agent.SetDestination(target.position);
        agent.stoppingDistance = 1f;
        
        //was having trouble with the BT continuing before the navmeshagent reaches its destination, i referred to these forum: https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403/5,
        //https://forum.unity.com/threads/navmeshpathstatus-is-always-pathcomplete.396390/ to check if the agent reached its destination

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ThisTask.Succeed();
        }
    }

    [Task]
    void TextSetActive(bool status) //pass in from AgentBT
    {
        interactTxtGO.SetActive(status); //set true or false depending on parameter
        ThisTask.Succeed();
    }

    [Task]
    void Idle()
    {
        interactTxt.text = "E to interact"; //reset text back to default when going back to idle
        ThisTask.Succeed();
    }

    //interaction at counter
    [Task]
    void GiveChoices()
    {
        interactTxt.text = chef.questIsDone ? "1 - Order/SPACE - Bye" : "1 - Order/2 - Quest/SPACE - Bye"; //since quest cannot be done again, the option is removed
        if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetKeyDown(KeyCode.Alpha2) && !chef.questIsDone) || Input.GetKeyDown(KeyCode.Space))
        {
            ThisTask.Succeed();
        }
    }

    [Task]
    void StopPlayer(bool status)
    {
        player.stopPlayer = status; 
        ThisTask.Succeed();
    }

    [Task]
    void ChangeText(string text) 
    {
        interactTxt.text = text;
        ThisTask.Succeed();
    }

    //quest
    [Task]
    bool InQuest()
    {
        //abort quest if its angry
        if (chef.isAngry) return false;
        return chef.isInQuest; //pass through the quest tree when this is returned true
    }

    [Task]
    void StartQuest()
    {
        chef.isInQuest = true;
        ThisTask.Succeed();
    }

    [Task]
    void EndQuest()
    {
        chef.isInQuest = false;
        ThisTask.Succeed();
    }

    [Task]
    bool QuestIsDone()
    {
        return chef.questIsDone; //to stop the quest from starting again
    }

    //preparing order
    [Task]
    void StartOrder()
    {
        chef.isPreparingOrder = true; 
        ThisTask.Succeed();
    }

    [Task]
    void EndOrder()
    {
        chef.isPreparingOrder = false;
        ThisTask.Succeed();
    }

    [Task]
    bool IsPreparingOrder()
    {
        //do not prepare order if its angry
        if (chef.isAngry) return false;
        return chef.isPreparingOrder; //pass through the prepare order tree when this is returned true
    }

    [Task]
    void Cook()
    {
        chef.isCooking = true; //starts the cooking in chef controller update
        if (chef.cookingTime >= 100f)
        {
            chef.cookingTime = 0f; //to reset the cookingtime once cooking is completed
            chef.isCooking = false; 

            if (chef.isAngry) //so that if chef is angry. it will immediately chase the player even if its cooking (as without it, it will go to counter before chasing the player)
            {
                ThisTask.Fail();
                return;
            }

            ThisTask.Succeed();
        }
    }

    //preparing order - stock & refilling
    [Task]
    bool HasCrops()
    {
        return chef.cropsLeft > 0;
    }

    [Task]
    bool TakeCrop()
    {
        chef.TakeCrops(); //call take crops function in chef controller
        return true;
    }

    [Task]
    void HarvestCrops()
    {
        chef.fillCropStock(); //call refill crop function in chef controller
        ThisTask.Succeed();
    }

    [Task]
    bool HasMeat()
    {
        return chef.meatLeft > 0;
    }

    [Task]
    bool TakeMeat()
    {
        chef.TakeMeat(); //call take meat function in chef controller
        return true;
    }

    [Task]
    void HarvestMeat()
    {
        chef.fillMeatStock(); //call refill meat function in chef controller
        ThisTask.Succeed();
    }

    //angry chef
    [Task]
    bool IsAngry()
    {
        agent.speed = chef.isAngry ? agentRunSpeed : agentWalkSpeed; //to set navmeshagent speed to the bigger speed to chase after the player when its angry
        return chef.isAngry;
    }

    [Task]
    void ExitAngry()
    {
        chefsGold.SetActive(true); //activate the gold again
        chef.isAngry = false;
        ThisTask.Succeed();
    }

    [Task]
    bool IsFoodBurnt()
    {
        return chef.isFoodBurnt;
    }
}
