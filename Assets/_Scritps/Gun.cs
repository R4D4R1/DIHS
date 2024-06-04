using UnityEngine;
using EasyButtons;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Security.Cryptography;

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
    //[SerializeField] protected GameObject casingPref;
    [SerializeField] protected GameObject bulletHole;
    [SerializeField] protected TextMeshProUGUI ammoText;

    [Space(30)]

    [Header("Sounds")]
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip noBulletSound;
    [SerializeField] protected AudioClip reloadingSound;
    [SerializeField] protected AudioClip metalImpactSound;

    [Space(30)]

    [Header("Muzzle")]
    [SerializeField] protected ParticleSystem particleSystem;
    [SerializeField] protected Transform  muzzlePointTransform;

    [Space(30)]

    [Header("DifferentHandsAttach")]
    [SerializeField] private Transform leftHandAttachTransform;
    [SerializeField] private Transform rightHandAttachTransform;

    [Space(30)]

    [Header("Ammo")]
    [SerializeField] protected bool hasMagazineInside;
    protected int ammoInMagazine = 0;
    protected int ammoSpare = 999;

    protected RaycastHit hit;

    private void Start()
    {
        ammoText.text = ammoInMagazine.ToString() + "/" + ammoSpare.ToString();
    }

    public int GetAmmo()
    {
        return ammoInMagazine;
    }

    public void SetAmmoInMagazine(int ammo)
    {
        ammoInMagazine = ammo;
        ammoText.text = ammoInMagazine.ToString() + "/" + ammoSpare.ToString();
    }

    [Button]
    public abstract void ShootGun();


    public void DecreaseAmmo()
    {
        if(ammoInMagazine>0)
        {
            ammoInMagazine--;
            ammoText.text = ammoInMagazine.ToString() + "/" + ammoSpare.ToString();
        }
    }

    // public void Reload()
    // {
    //     if(ammoInMagazine<ammoBaseInMag)
    //     {

    //     }
    // }

    public void ProcessRecoil()
    {
        particleSystem.Play();

        if(recoilBody != null)
        {
            recoilBody.AddForce(-transform.forward*recoilForce,ForceMode.Impulse);

            //Вторая рука взята
            if(secondHandRigidbody != null)
            {
                recoilForce *=0.1f;
                secondHandRigidbody.AddForce(-transform.right * recoilForce,ForceMode.Impulse);
            }
            else
                recoilBody.transform.localRotation = Quaternion.AngleAxis(-2 * recoilForce,Vector3.right);
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
                        bulletHoleElementInstance.transform.localPosition = hit.point + hit.normal.normalized*0.01f;

                        //CHANGE BEHAVIOR ON IMPACT SOUND
                        if((int)hit.collider.gameObject.GetComponent<Target>().ImpactMaterial == 1)
                        {
                            bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = metalImpactSound;
                        }
                        bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = metalImpactSound;

                        bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().Play();
                        break;
                    }
                    else
                    {
                        if(i==BulletPoolManager.instance.bulletHoleList.Count-1) // Last Bullet 
                        {
                            GameObject newBulletHole = Instantiate(BulletPoolManager.instance.bulletHolePrefab);
                            newBulletHole.transform.parent = BulletPoolManager.instance.transform;
                            newBulletHole.SetActive(false);
                            
                            //CHANGE BEHAVIOR ON IMPACT SOUND
                            bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = metalImpactSound;
                            bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().Play();

                            BulletPoolManager.instance.bulletHoleList.Add(newBulletHole);
                        }
                    }
                }



                //TakeDamage
            if(hit.collider.gameObject.GetComponent<Target>())
            {            
                Target target = hit.transform.GetComponent<Target>();
                if(target != null )
                {
                    target.TakeDamage(damage);
                }
            }

        }
    }


    // XR INTERACTION CHANGES

         public void GrabWeapon(SelectEnterEventArgs grabData)
        {
            var hand = grabData.interactorObject;
            var child = hand.transform.GetChild(0);
            recoilBody = child.GetComponent<Rigidbody>();

            Debug.Log(recoilBody);

        }
 
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = leftHandAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = rightHandAttachTransform;
        }
 
        base.OnSelectEntering(args);
    }

}
