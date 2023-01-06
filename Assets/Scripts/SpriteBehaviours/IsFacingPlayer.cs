using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFacingPlayer : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 initialRotation;

    private void Start()
    {
        playerTransform = PlayerContext.instance.transform;
        initialRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(newPosition);
        transform.forward = -transform.forward;
        //transform.rotation = Quaternion.Euler(initialRotation.x, playerTransform.rotation.eulerAngles.y, initialRotation.z);
    }
}
