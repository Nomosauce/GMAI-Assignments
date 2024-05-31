using UnityEngine;
using Panda;
using System.Runtime.CompilerServices;
using UnityEngine.AI;
using TMPro;
using System.Drawing;
using System.Linq;

public class ChefAgentTasks : MonoBehaviour
{
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
        //abort going if its angry after chasing the player or going back to table
        if (chef.isAngry && !(tag.Equals("Player") || tag.Equals("Table") || tag.Equals("Trash") || tag.Equals("Stove"))) 
        {
            ThisTask.Succeed();
        }

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
    void TextSetActive(bool status)
    {
        interactTxtGO.SetActive(status);
        ThisTask.Succeed();
    }

    [Task]
    void Idle()
    {
        interactTxt.text = "E to interact";
        ThisTask.Succeed();
    }

    //interaction at counter
    [Task]
    void GiveChoices()
    {
        interactTxt.text = "1 - Order/2 - Quest/SPACE - Bye";
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Space))
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
        return chef.isInQuest;
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
        return chef.questIsDone;
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
        //do not prepare order of its angry
        if (chef.isAngry) return false;

        return chef.isPreparingOrder;
    }

    [Task]
    void Cook()
    {
        chef.isCooking = true;
        if (chef.cookingTime >= 100f)
        {
            chef.cookingTime = 0f;
            chef.isCooking = false;
            if (chef.isAngry)
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
        if (chef.cropsLeft > 0)
        {
            return true;
        }
        return false;
    }

    [Task]
    bool TakeCrop()
    {
        chef.TakeCrops();
        return true;
    }

    [Task]
    void HarvestCrops()
    {
        chef.fillCropStock();
        ThisTask.Succeed();
    }

    [Task]
    bool HasMeat()
    {
        if (chef.meatLeft > 0) return true;
        return false;
    }

    [Task]
    bool TakeMeat()
    {
        chef.TakeMeat();
        return true;
    }

    [Task]
    void HarvestMeat()
    {
        chef.fillMeatStock();
        ThisTask.Succeed();
    }

    //angry chef
    [Task]
    bool IsAngry()
    {
        agent.speed = chef.isAngry ? 12f : 6f;
        return chef.isAngry;
    }

    [Task]
    void ExitAngry()
    {
        chefsGold.SetActive(true);
        chef.isAngry = false;
        ThisTask.Succeed();
    }

    [Task]
    bool IsFoodBurnt()
    {
        return chef.isFoodBurnt;
    }
}
