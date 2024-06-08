using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector;
    float time = 0;

    void OnEnable()
    {
        Invoke("OnDisable",BulletPoolManager.instance.bulletDestroyTime);

    }

    private void Update()
    {
        float val = Mathf.Lerp(1f,0f,time);
        time += Time.deltaTime/BulletPoolManager.instance.bulletDestroyTime;
        
        decalProjector.fadeFactor = val;

        
    }

    private void OnDisable()
    {
        this.transform.gameObject.SetActive(false);
    }

}
