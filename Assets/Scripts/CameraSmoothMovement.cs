using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothMovement : MonoBehaviour
{

    public Transform player;

    public float smoothSpeed;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    Quaternion playerRotation;

    float playerYRotation;

    void FixedUpdate()
    {

        playerYRotation = player.rotation.eulerAngles.y;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;

        transform.rotation = Quaternion.Euler(new Vector3(0, playerYRotation, 0));

    }
}
