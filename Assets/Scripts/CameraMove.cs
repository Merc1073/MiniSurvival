using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform player;

    private Vector3 velocity = Vector3.zero;

    public float smoothSpeed;
    public Vector3 offset;


    private void Update()
    {

        //transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, 0.2f);
        //transform.position = player.position;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = player.position;


        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0, -45, 0);
            //player.transform.Rotate(0, -45, 0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0, 45, 0);
            //player.transform.Rotate(0, 45, 0);
        }
    }
}
