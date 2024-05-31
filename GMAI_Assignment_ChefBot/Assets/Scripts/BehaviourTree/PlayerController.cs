using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //3d movement code by brackeys: https://www.youtube.com/watch?v=4HpC--2iowE&t=675s
    [Header("Player Configuration")]
    private CharacterController controller;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public bool stopPlayer;

    [Header("Others")]
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
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;  //shortened if-statement which decides which speed var the player uses, when holding shift key it uses the run speed, and walk speed when not
            controller.Move(direction * currentSpeed * Time.deltaTime);
        }

        if (!chef.isAngry && withinStealingRange()) //to give the player the option to steal the chefs gold, which will trigger its angry mode
        {
            stealText.SetActive(true); //shows the steal UI

            if (Input.GetKeyDown(KeyCode.E)) //the players input will only be checked when theyre near the table where the gold is, and only when the chef isnt already angered
            {
                stealGold(); 
            }
        }
        else
        {
            stealText.SetActive(false); //deactivates the steal UI
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) //since the player uses character controller component it cannot detect collision boxes
    {
        if (hit.gameObject.CompareTag("Monster"))
        {
            chef.questIsDone = true; //completes the quest (permenently) and disables the monster upon touch
            monster.SetActive(false);
        }
    }

    bool withinStealingRange() //bool function to check if the player is near the table (where gold is kept)
    {
        Transform table = GameObject.FindGameObjectWithTag("Table").transform; //since we only need the position of the table for this function only its Transform is stored as a local var

        if (Vector3.Distance(gameObject.transform.position, table.position) <= 5f) //vector.dist is used to find the dist between the first parameter and the second
        {
            return true; //returns true if distance between player and table is below or equal to 5
        }
        return false; //returns false if above 5
    }

    void stealGold() //function that player calls when their input is detected
    {
        chef.chefsGold.SetActive(false); //disable the gold gameobject to make it seem like it was stolen
        stealText.SetActive(false); //disable text UI here because i did not want to use pandaBT task function for the player
        chef.isAngry = true;

        //monster.SetActive(false); //personally wouldnt want the monster from the quest to stay put since its meant to only exist within the quest
    }
}
