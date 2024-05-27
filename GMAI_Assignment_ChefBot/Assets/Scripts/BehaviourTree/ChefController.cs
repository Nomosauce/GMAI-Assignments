using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    public bool isInQuest = false;
    public bool monsterDefeat = false;
    public bool isPreparingOrder = false;
    public bool isCooking = false;

    public GameObject monster;
    public GameObject[] cropStock;

    public int cropsLeft = 0;
    public int cropStockIndex = 0;

    public  float cookingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        monster.SetActive(false);

        cropStock = GameObject.FindGameObjectsWithTag("Crop");
        foreach (var item in cropStock)
        {
            cropsLeft++;
        }
    }

    void Update()
    {
        if (!monsterDefeat && isInQuest)
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
}
