using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameScript : MonoBehaviour
{


    public int foodSpawnAmount = 0;

    public int foodCounter;

    public GameObject foodPrefab;

    public List<Food> allFoods = new List<Food>();

    public List<GameObject> foodObject;


    void Start()
    {
        foodCounter = foodSpawnAmount;

        for(int i = 0; i < foodSpawnAmount; i++)
        {
            GameObject clone;
            Vector3 position = new Vector3(Random.Range(-40, 40), 0.5f, Random.Range(-30, 30));

            clone = Instantiate(foodPrefab, position, Quaternion.Euler(0, 0, 0));
            allFoods.Add(new Food(clone, position));
            allFoods[i].entity.name = allFoods[i].entity.name + i;
        }

    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {

        if(foodCounter < 0)
        {
            foodCounter = 0;
        }

        allFoods.RemoveAll(obj => obj == null);
        foodObject.RemoveAll(obj => obj == null);

        if(other.gameObject.CompareTag("Food"))
        {
            foodObject.Add(other.gameObject);
        }


    }

    public void ReduceFood()
    {
        foodCounter--;
    }

}
