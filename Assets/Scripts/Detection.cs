using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public List<Transform> food;

    public Transform targetFood;

    public List<GameObject> foodObject;

    public GameObject targetFoodObject;


    void Start()
    {
        targetFood = null;
        targetFoodObject = null;
    }

    void Update()
    {
        food.RemoveAll(i => i == null);
        foodObject.RemoveAll(i => i == null);

        if(food.Count != 0)
        {
            targetFood = GetClosestTransform(food);

            targetFoodObject = GetClosestObject(foodObject);
        }

        else
        {
            targetFood = null;
            targetFoodObject = null;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            food.Add(other.transform);
            foodObject.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Food"))
        {
            food.Remove(other.transform);
            foodObject.Remove(other.gameObject);
        }
    }


    Transform GetClosestTransform(List<Transform> items)
    {

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in items)
        {

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }

        }

        return bestTarget;
    }




    GameObject GetClosestObject(List<GameObject> items)
    {

        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in items)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }



}
