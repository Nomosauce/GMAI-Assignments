using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Rendering;

public class ChefController : MonoBehaviour
{
    [Header("Quest")]
    public GameObject monster;
    
    public bool isInQuest = false;
    public bool questIsDone = false;

    [Header("Angry")]
    public GameObject chefsGold;

    public bool isAngry = false;

    [Header("Ingredients")]
    public GameObject[] cropStock;
    public GameObject[] meatStock;

    public int cropsLeft = 0;
    public int cropStockIndex = 0;
    public int meatLeft = 0;
    public int meatStockIndex = 0;

    [Header("Cooking")]
    public bool isPreparingOrder = false;
    public bool isCooking = false;
    public bool isFoodBurnt = false;

    public float cookingTime = 0f;
    public float cookingSpeed = 15f;

    public int burntChance = 3;

    // Start is called before the first frame update
    void Start()
    {
        monster.SetActive(false); //as i rather have the monster exist only within the quest, he is first set to false

        cropStock = GameObject.FindGameObjectsWithTag("Crop"); //fill arrays with the crop/meat gameobjects tagged in scene
        meatStock = GameObject.FindGameObjectsWithTag("Meat"); 
        
        cropsLeft = cropStock.Length; //represents the initial number of crop/meat tagged gameobjects in scene by setting it to the size of the array
        meatLeft = meatStock.Length;
    }

    void Update()
    {
        if (!questIsDone && isInQuest) //enables the monster gameobject if the quest has not been done before and if the player is currently in a quest
        {
            monster.SetActive(true);
        }
        else //makes the monster only visible in a quest
        {
            monster.SetActive(false);
        }

        if (isCooking) //as long as it is cooking, it will continuely increment by 15 until it first reaches 100, where the cookingtime will be set to 100 before being returned to the BT
        {
            cookingTime += cookingSpeed * Time.deltaTime;

            if (cookingTime >= 100f || isAngry) //if the chef is angry the cooking skips to being done as chasing the player interrupts its cooking action 
            {
                cookingTime = 100f;
                if (!isAngry) return; //to simulate how the food burns after being left on the stove unattended (like its still cooking) from chasing the player, only when its angry, it will randomise if the chef has returned to burnt food or not
                isFoodBurnt = Random.Range(0, 10) <= burntChance ? true : false;
            }
        }

        if (isAngry) //turn off the quest and order booleans
        {
            isInQuest = false;
            isPreparingOrder = false;
        }
    }

    public void TakeCrops() //for disabling the crop ingredient gameobjects on the scene one by one in the array, by increasing the index everytime the bot uses the crops to show that it is visibly running out
    {
        if (cropsLeft > 0) //only reduce when there are still crops left
        {
            cropsLeft -= 1;
            cropStock[cropStockIndex].SetActive(false);
            cropStockIndex++;
        }
    }

    public void TakeMeat() //for disabling the meat ingredient gameobjects on the scene one by one in the array, by increasing the index everytime the bot uses the meat to show that it is visibly running out
    {
        if (meatLeft > 0) //only reduce when there are still meat left
        {
            meatLeft -= 1;
            meatStock[meatStockIndex].SetActive(false);
            meatStockIndex++;
        }
    }

    public void fillCropStock() //for resetting the index back to 0, the crops left back to the maximum number of crop in the scene, and to enable all the crop ingredients in the scene
    {
        cropStockIndex = 0;
        cropsLeft = cropStock.Length;
        for (int i = 0; i < cropStock.Length; i++)
        {
            cropStock[i].SetActive(true);
        }
    }

    public void fillMeatStock() //for resetting the index back to 0, the meat left back to the maximum number of meat in the scene, and to enable all the meat ingredients in the scene
    {
        meatStockIndex = 0;
        meatLeft = meatStock.Length;
        for (int i = 0; i < meatStock.Length; i++)
        {
            meatStock[i].SetActive(true);
        }
    }
}
