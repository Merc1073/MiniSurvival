using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public List<Transform> grass;
    public List<Transform> twigs;

    public Transform targetGrass;
    public Transform targetTwigs;

    public List<GameObject> grassObject;
    public List<GameObject> twigsObject;

    public GameObject targetGrassObject;
    public GameObject targetTwigsObject;


    void Start()
    {
        targetGrass = null;
        targetGrassObject = null;

        targetTwigs = null;
        targetTwigsObject = null;
    }

    void Update()
    {
        grass.RemoveAll(i => i == null);
        grassObject.RemoveAll(i => i == null);

        twigs.RemoveAll(i => i == null);
        twigsObject.RemoveAll(i => i == null);

        if (grass.Count != 0)
        {
            targetGrass = GetClosestTransform(grass);

            targetGrassObject = GetClosestObject(grassObject);
        }

        else
        {
            targetGrass = null;
            targetGrassObject = null;
        }


        if (twigs.Count != 0)
        {
            targetTwigs = GetClosestTransform(twigs);

            targetTwigsObject = GetClosestObject(twigsObject);
        }

        else
        {
            targetTwigs = null;
            targetTwigsObject = null;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grass"))
        {
            grass.Add(other.transform);
            grassObject.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Twigs"))
        {
            twigs.Add(other.transform);
            twigsObject.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Grass"))
        {
            grass.Remove(other.transform);
            grassObject.Remove(other.gameObject);
        }

        if (other.gameObject.CompareTag("Twigs"))
        {
            twigs.Remove(other.transform);
            twigsObject.Remove(other.gameObject);
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
