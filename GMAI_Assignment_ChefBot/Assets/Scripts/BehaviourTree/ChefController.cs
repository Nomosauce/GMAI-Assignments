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
        monster.SetActive(false);

        cropStock = GameObject.FindGameObjectsWithTag("Crop");
        foreach (var item in cropStock)
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
        if (!questIsDone && isInQuest)
        {
            monster.SetActive(true);
        }

        if (isCooking)
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

    public void TakeCrops()
    {
        if (cropsLeft > 0)
        {
            cropsLeft -= 1;
            cropStock[cropStockIndex].SetActive(false);
            cropStockIndex++;
        }
    }

    public void fillCropStock()
    {
        cropStockIndex = 0;
        cropsLeft = cropStock.Length;
        for (int i = 0; i < cropStock.Length; i++)
        {
            cropStock[i].SetActive(true);
        }
    }

    public void TakeMeat()
    {
        if (meatLeft > 0)
        {
            meatLeft -= 1;
            meatStock[meatStockIndex].SetActive(false);
            meatStockIndex++;
        }
    }

    public void fillMeatStock()
    {
        meatStockIndex = 0;
        meatLeft = meatStock.Length;
        for (int i = 0; i < meatStock.Length; i++)
        {
            meatStock[i].SetActive(true);
        }
    }
}
