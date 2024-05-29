using UnityEngine;
using Panda;
using System.Runtime.CompilerServices;
using UnityEngine.AI;
using TMPro;

public class ChefAgentTasks : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject interactTxtGO;
    public TMP_Text interactTxt;

    public ChefController chef;
    public PlayerController player;

    //general
    [Task]
    bool IsNear(string tag1, string tag2) //task that checks if two objects are close to each other by getting the Transform (since only position is needed) component of the tagged objects
    {
        Transform obj1 = GameObject.FindGameObjectWithTag(tag1).transform;
        Transform obj2 = GameObject.FindGameObjectWithTag(tag2).transform;
        float distance = 2f;

        return Vector3.Distance(obj1.position, obj2.position) <= distance; //vectorn.Distance method return the difference between the positions of the gameobjects and compares them to the set distance 
    }

    [Task]
    void GoTo(string tag)
    {
        Transform target = GameObject.FindGameObjectWithTag(tag).transform;
        agent.SetDestination(target.position);
        agent.stoppingDistance = 1f;
        if (chef.isAngry)
        {
            agent.speed = 12f;
        }
        else
        {
            agent.speed = 6f;
        }

        //was having trouble with the BT continuing before the navmeshagent reaches its destination, i referred to these forum: https://discussions.unity.com/t/how-can-i-tell-when-a-navmeshagent-has-reached-its-destination/52403/5,
        //https://forum.unity.com/threads/navmeshpathstatus-is-always-pathcomplete.396390/ to check if the agent reached its destination

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ThisTask.Succeed();
        }
        else if (chef.isAngry)
        {
            ThisTask.Fail();
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
            ThisTask.Succeed();
        }
        else if (chef.isAngry)
        {
            chef.isCooking = false;
            ThisTask.Fail();
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
        if (chef.meatLeft > 0)
        {
            return true;
        }
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
        return chef.isAngry;
    }
}
