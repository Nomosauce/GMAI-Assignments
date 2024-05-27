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
    public PlayerMovement player;

    [Task]
    bool IsNear(string tag1, string tag2) //task that checks if two objects are close to each other by getting the Transform (since only position is needed) component of the tagged objects
    {
        Transform obj1 = GameObject.FindGameObjectWithTag(tag1).transform;
        Transform obj2 = GameObject.FindGameObjectWithTag(tag2).transform;
        float distance = 2.5f;

        return Vector3.Distance(obj1.position, obj2.position) <= distance; //vectorn.Distance method return the difference between the positions of the gameobjects and compares them to the set distance 
    }

    [Task]
    void GoTo(string tag)
    {
        Transform target = GameObject.FindGameObjectWithTag(tag).transform;
        agent.SetDestination(target.position);
        agent.stoppingDistance = 2f;

        ThisTask.Succeed();
    }

    [Task]
    void TextSetActive(bool status)
    {
        interactTxtGO.SetActive(status);
        ThisTask.Succeed();
    }

    [Task]
    void GiveChoices()
    {
        interactTxt.text = "1 - Food/2 - Drink/3 - Quest/SPACE - Bye";
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Space))
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

    [Task]
    void Idle()
    {
        interactTxt.text = "E to interact";
        ThisTask.Succeed();
    }

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
    bool MonsterDefeated()
    {
        return chef.monsterDefeat;
    }

    [Task]
    void EndQuest()
    {
        chef.isInQuest = false;
        ThisTask.Succeed();
    }

    [Task]
    void SetOrder(bool status)
    {
        chef.isPreparingOrder = status;
        ThisTask.Succeed();
    }

    [Task]
    bool IsPreparingOrder()
    {
        return chef.isPreparingOrder;
    }

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
    void Cook()
    {
        chef.isCooking = true;
        if (chef.cookingTime >= 100f)
        {
            chef.cookingTime = 0f;
            chef.isCooking = false;
            ThisTask.Succeed();
        }
    }

    [Task]
    void HarvestCrops()
    {
        chef.fillCropStock();
        ThisTask.Succeed();
    }
}
