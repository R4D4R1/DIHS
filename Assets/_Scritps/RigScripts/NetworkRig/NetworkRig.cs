using UnityEngine;
using Mirror;
public class NetworkRig : NetworkBehaviour
{
    public HardwareRig hardwareRig;

    public NetworkHand leftNetworkHand;
    public NetworkHand rightNetworkHand;
    public NetworkHead networkHead;
    public NetworkBody networkBody;

    [SerializeField] private float offset;

    private void Start()
    {     
        if(isLocalPlayer)
        {
            hardwareRig = GameObject.FindWithTag("Player").GetComponent<HardwareRig>();
        }
    }

    private void LateUpdate()
    {
        if(hardwareRig != null)
        {
            transform.SetPositionAndRotation(hardwareRig.transform.position, hardwareRig.transform.rotation);
            leftNetworkHand.transform.SetPositionAndRotation(hardwareRig.leftHandPosition, hardwareRig.leftHandRotation);
            rightNetworkHand.transform.SetPositionAndRotation(hardwareRig.rightHandPosition, hardwareRig.rightHandRotation);
            networkHead.transform.SetPositionAndRotation(hardwareRig.headsetPosition, hardwareRig.headsetRotation);

            networkBody.transform.position = new Vector3(hardwareRig.bodyPosition.x, hardwareRig.bodyPosition.y + offset, hardwareRig.bodyPosition.z);
            networkBody.transform.eulerAngles = new Vector3(0f, hardwareRig.bodyRotation.eulerAngles.y , 0f);



        }
    }
}
