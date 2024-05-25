using UnityEngine;
using Panda;
using System.Runtime.CompilerServices;
using UnityEngine.AI;

public class ChefAgentTasks : MonoBehaviour
{
    public NavMeshAgent agent;

    [Task]
    bool isNear(string tag1, string tag2)
    {
        Transform obj1 = GameObject.FindGameObjectWithTag(tag1).transform;
        Transform obj2 = GameObject.FindGameObjectWithTag(tag2).transform;
        float distance = 2.5f;

        return Vector3.Distance(obj1.position, obj2.position) <= distance;
    }

    [Task]
    void goTo(string tag)
    {
        Transform target = GameObject.FindGameObjectWithTag(tag).transform;
        agent.SetDestination(target.position);
        agent.stoppingDistance = 2f;

        ThisTask.Succeed();
    }

    [Task]
    bool idle()
    {
        return true;
    }
}
