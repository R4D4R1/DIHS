using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardwareRig : MonoBehaviour
{
    public HardwareHand leftHand;
    public HardwareHand rightHand;
    public HardwareHead headSet;

    public Vector3 playerPosition;
    public Quaternion playerRotation;

    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;

    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;

    public Vector3 headsetPosition;
    public Quaternion headsetRotation;

    private void LateUpdate()
    {
        playerPosition = transform.position;
        playerRotation = transform.rotation;
        leftHandPosition = leftHand.transform.position;
        leftHandRotation = leftHand.transform.rotation;
        rightHandPosition = rightHand.transform.position;
        rightHandRotation = rightHand.transform.rotation;
        headsetPosition = headSet.transform.position;
        headsetRotation = headSet.transform.rotation;
    }

}
