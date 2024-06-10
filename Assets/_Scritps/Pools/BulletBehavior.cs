using UnityEngine;
using UnityEngine.Rendering.Universal;


public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector;
    private bool isAvaliable = false;
    private float time = 0;

    private void Update()
    {
        if(isAvaliable)
        {

            time = 0;
            float val = Mathf.Lerp(1f, 0f, time);
            time += Time.deltaTime / BulletPoolManager.instance.bulletDestroyTime;

            decalProjector.fadeFactor = val;

            if(Mathf.Approximately(val, 0)) 
            {
                isAvaliable = false;
            }
        }
      
    }

    public void StartHit()
    {
        isAvaliable = true;
    }

    public bool IsAvaliable()
    {
        return isAvaliable;
    }

}
