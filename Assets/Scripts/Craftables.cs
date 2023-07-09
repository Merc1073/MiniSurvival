using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craftables : MonoBehaviour
{
    public Transform This;
    public GameObject entity;

    public Vector3 position;


    public Craftables(GameObject _entity, Vector3 _position)
    {

        entity = _entity;
        This = entity.transform;
        position = _position;
        entity.name = "Food";

    }
}
