
using System.Collections;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("OnDisable",BulletPoolManager.instance.bulletDestroyTime);

    }

    private void OnDisable()
    {
        this.transform.gameObject.SetActive(false);
    }

}
