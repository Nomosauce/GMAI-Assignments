using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    public bool isInQuest = false;
    public bool questIsDone = false;
    public bool isPreparingOrder = false;
    public bool isCooking = false;
    public bool isAngry = false;

    public GameObject monster;
    public GameObject[] cropStock;
    public GameObject[] meatStock;
    public GameObject chefsGold;

    public int cropsLeft = 0;
    public int cropStockIndex = 0;
    public int meatLeft = 0;
    public int meatStockIndex = 0;

    public  float cookingTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        monster.SetActive(false); //as i rather have the monster exist only within the quest, he is first set to false

        cropStock = GameObject.FindGameObjectsWithTag("Crop"); //fill the crop and meat arrays with all game objects that have that tag
        foreach (var item in cropStock) //run through each element in these arrays to increade the count of the tagged gameobjects that are in the scene, to prevent mistakes from hardcoding
        {
            cropsLeft++;
        }
        meatStock = GameObject.FindGameObjectsWithTag("Meat");
        foreach (var item in meatStock)
        {
            meatLeft++;
        }
    }

    void Update()
    {
        if (!questIsDone && isInQuest) //enables the monster gameobject if the quest has not been done before and if the player is currently in a quest
        {
            monster.SetActive(true);
        }

        if (isCooking) //as long as it is cooking, it will continuely increment by 15 until it first reaches 100, where the cookingtime will remain 100 and is returned to be used in the BT
        {
            cookingTime += 15f * Time.deltaTime;
            if (cookingTime >= 100f)
            {
                cookingTime = 100f;
                return;
            }
        }

        if (isAngry)
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
