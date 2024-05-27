using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //3d movement code by brackeys: https://www.youtube.com/watch?v=4HpC--2iowE&t=675s
    private CharacterController controller;
    private float playerSpeed = 6f;
    public bool stopPlayer;

    public GameObject monster;
    public ChefController chef;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        stopPlayer = false;
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
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Monster"))
        {
            chef.monsterDefeat = true;
            monster.SetActive(false);
        }
    }
}
