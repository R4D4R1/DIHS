using UnityEngine;
using EasyButtons;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public abstract class Gun : XRGrabInteractable
{
    
    [Header("Main Stats")]
    [SerializeField] protected float damage;

    [Space(30)]

    [Header("Recoil")]
    [SerializeField] protected float recoilForce;
    protected Rigidbody recoilBody;
    protected Rigidbody secondHandRigidbody;

    [Space(30)]

    [Header("Prefab References")]
    [SerializeField] protected GameObject casingPref;
    [SerializeField] protected GameObject bulletHole;

    [Space(30)]

    [Header("Sounds")]
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip noBulletSound;
    [SerializeField] protected AudioClip ReloadingSound;

    [Space(30)]

    [Header("Muzzle")]
    [SerializeField] protected ParticleSystem particleSystem;
    [SerializeField] protected Transform  muzzlePointTransform;

    protected RaycastHit hit;


    [Button]
    public abstract void ShootGun();

    public void ProcessRecoil()
    {
        if(recoilBody != null)
        {
            recoilBody.AddForce(-transform.right*recoilForce,ForceMode.Impulse);

            //Вторая рука взята
            if(secondHandRigidbody != null)
            {
                recoilForce *=0.1f;
                secondHandRigidbody.AddForce(-transform.right * recoilForce,ForceMode.Impulse);
            }
            else
            recoilBody.transform.localRotation = Quaternion.AngleAxis(-1 * recoilForce,Vector3.right);
        }
    }

    public void HitSomething(Vector3 offset)
    {
        if(Physics.Raycast(muzzlePointTransform.position,muzzlePointTransform.forward + offset, out hit, 100))
        {
            
            if (hit.collider.gameObject.GetComponent<Collider>())
                for(int i = 0;i < BulletPoolManager.instance.bulletHoleList.Count; i++)
                {
                    var bulletHoleElementInstance = BulletPoolManager.instance.bulletHoleList[i];
                if(BulletPoolManager.instance.bulletHoleList[i].activeInHierarchy == false)
                    {

                        bulletHoleElementInstance.SetActive(true);
                        bulletHoleElementInstance.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        bulletHoleElementInstance.transform.localPosition = hit.point + new Vector3(0f, 0f, hit.transform.forward.z*0.01f);

                        break;
                    }
                    else
                    {
                        if(i==BulletPoolManager.instance.bulletHoleList.Count-1) // Last Bullet
                        {
                            GameObject newBulletHole = Instantiate(BulletPoolManager.instance.bulletHolePrefab);
                            newBulletHole.transform.parent = BulletPoolManager.instance.transform;
                            newBulletHole.SetActive(false);

                            BulletPoolManager.instance.bulletHoleList.Add(newBulletHole);
                        }
                    }
                }




            if(hit.collider.gameObject.GetComponent<Player>())
            {            
                Player player = hit.transform.GetComponent<Player>();
                if(player != null )
                {
                    player.TakeDamage(damage);
                }
            }

        }
    }

         public void GrabWeapon(SelectEnterEventArgs grabData)
        {
            var hand = grabData.interactorObject;
            var child = hand.transform.GetChild(0);
            recoilBody = child.GetComponent<Rigidbody>();

            Debug.Log(recoilBody);

        }

}
