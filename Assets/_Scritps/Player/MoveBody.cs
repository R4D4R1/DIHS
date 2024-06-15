using UnityEngine;

public class MoveBody : MonoBehaviour
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private float offsetY;


    private void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y + offsetY, headTransform.position.z);
        transform.localRotation = new Quaternion(0f, headTransform.localRotation.y, 0f,1f);
    }
}
