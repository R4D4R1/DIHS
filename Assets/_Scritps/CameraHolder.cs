using UnityEngine;

public class CameraHolder : MonoBehaviour
{

    [SerializeField] private Transform cameraTrans;


    void Update()
    {
        transform.position = cameraTrans.position;
    }
}
