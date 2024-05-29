using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //3d movement code by brackeys: https://www.youtube.com/watch?v=4HpC--2iowE&t=675s
    private CharacterController controller;
    private float playerSpeed = 6f;
    public bool stopPlayer;

    public GameObject stealText;
    public GameObject monster;

    public ChefController chef;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        stopPlayer = false;
        stealText.SetActive(false);
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //this returns a value from -1 to 1 when pressing on the a/left (-1) or d/right keys (1), no input returns 0
        float vertical = Input.GetAxisRaw("Vertical"); //this returns a value from -1 to 1 when pressing on the s/down (-1) or w/up keys (1), no input returns 0
                                                       //it uses axis raw for no input smoothing, so oit goes from 0 to 1/-1 immediately without increase/decrease of 0.05f

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; //since we just need the direction and prevent the magnitude from messing with the calculation, it is normalised into a unit
                                                                              //(avoids player from speeding up when moving diagonally)

        if (!stopPlayer && direction.magnitude >= 0.1f) //checks if the player inputs is moving to any direction by checking the length of the direction vector
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(direction * (playerSpeed * 2) * Time.deltaTime); //doubles the speed whenever left shift is held down
            }
            else
            {
                controller.Move(direction * playerSpeed * Time.deltaTime);
            }
        }

        if (!chef.isAngry && withinStealingRange())
        {
            stealText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                stealGold();
            }
        }
        else
        {
            stealText.SetActive(false);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Monster"))
        {
            chef.questIsDone = true;
            monster.SetActive(false);
        }

        if (hit.gameObject.CompareTag("Chef"))
        {
            chef.isAngry = false;
            chef.chefsGold.SetActive(true);
        }
    }

    bool withinStealingRange()
    {
        Transform table = GameObject.FindGameObjectWithTag("Table").transform;

        if (Vector3.Distance(gameObject.transform.position, table.position) <= 5f)
        {
            return true;
        }
        return false;
    }

    void stealGold()
    {
        chef.chefsGold.SetActive(false);
        stealText.SetActive(false);
        chef.isAngry = true;

        monster.SetActive(false);
    }
}
