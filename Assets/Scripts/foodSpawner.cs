using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawner : MonoBehaviour
{

    public GameObject food;

    public int totalFood;
    public int foodCount;

    void Start()
    {
        for(int i = 0; i < totalFood; i++) 
        {
            GameObject clone;
            Vector3 position = new Vector3(Random.Range(20, -20), 0.5f, Random.Range(20, -20));
            clone = Instantiate(food, position, Quaternion.Euler(0, 0, 0));
            clone.name = food.name + " " + foodCount;
            foodCount++;
        }
        
    }

}
