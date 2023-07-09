using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameScript : MonoBehaviour
{


    public int grassSpawnAmount = 0;
    public int twigsSpawnAmount = 0;

    public int grassCounter;
    public int twigsCounter;

    public GameObject grassPrefab;
    public GameObject twigsPrefab;

    public List<Craftables> allGrass = new List<Craftables>();
    public List<Craftables> allTwigs = new List<Craftables>();

    public List<GameObject> grassObject;
    public List<GameObject> twigsObject;


    void Start()
    {
        grassCounter = grassSpawnAmount;
        twigsCounter = twigsSpawnAmount;

        for(int i = 0; i < grassSpawnAmount; i++)
        {
            GameObject clone;
            Vector3 position = new Vector3(Random.Range(-40, 40), 0.5f, Random.Range(-30, 30));

            clone = Instantiate(grassPrefab, position, Quaternion.Euler(0, 0, 0));
            allGrass.Add(new Craftables(clone, position));
            allGrass[i].entity.name = allGrass[i].entity.name + i;
        }

        for (int i = 0; i < twigsSpawnAmount; i++)
        {
            GameObject clone;
            Vector3 position = new Vector3(Random.Range(-40, 40), 0.5f, Random.Range(-30, 30));

            clone = Instantiate(twigsPrefab, position, Quaternion.Euler(0, 0, 0));
            allTwigs.Add(new Craftables(clone, position));
            allTwigs[i].entity.name = allGrass[i].entity.name + i;
        }

    }

    void Update()
    {
        if (grassCounter < 0)
        {
            grassCounter = 0;
        }

        if (twigsCounter < 0)
        {
            twigsCounter = 0;
        }

        allGrass.RemoveAll(obj => obj == null);
        grassObject.RemoveAll(obj => obj == null);

        allTwigs.RemoveAll(obj => obj == null);
        twigsObject.RemoveAll(obj => obj == null);

    }

    public void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Grass"))
        {
            grassObject.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Twigs"))
        {
            twigsObject.Add(other.gameObject);
        }

    }

    public void ReduceFood()
    {
        grassCounter--;
    }

}
