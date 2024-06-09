using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public abstract class Weapon : XRGrabInteractable
{

    [Header("Main Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected bool autoFireMode;


    protected Coroutine firingRoutine = null;
    protected WaitForSeconds wait = null;
    protected int ammoInMagazine = 0;
    protected GameObject magazine;
    protected bool magazineIsLoaded;

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

    [Header("Bolt")]
    [SerializeField] protected Transform boltTransform;
    [SerializeField] protected float targetX;
    private float startX;

    [Space(30)]

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip noBulletSound;
    [SerializeField] public AudioClip reloadingSound;
    [SerializeField] private LayerMask undestractableLayer;


    [Space(30)]

    [Header("Muzzle")]
    [SerializeField] protected ParticleSystem particleSystem;
    [SerializeField] protected Transform muzzlePointTransform;

    [Space(30)]

    [Header("DifferentHandsAttach")]
    [SerializeField] protected Transform leftHandAttachTransform;
    [SerializeField] protected Transform rightHandAttachTransform;

    [Space(30)]

    [Header("Two Hands Grab")]
    public List<XRSimpleInteractable> secondHandGrabpoints = new List<XRSimpleInteractable>();
    private IXRSelectInteractor secondInteractor;
    private Quaternion attachIntialRoation;
    public enum TwoHandRotationType { None, First, Second }
    public TwoHandRotationType twoHandRotationType;
    public bool SnapToSecondHand = true;
    private Quaternion intialRotationOffset;

    protected RaycastHit hit;

    private void Start()
    {
        startX = boltTransform.localPosition.x;

        wait = new WaitForSeconds(60 / fireRate);

        foreach (var item in secondHandGrabpoints)
        {
            item.selectEntered.AddListener(OnSecondHandGrab);
            item.selectExited.AddListener(OnSecondHandRelease);
        }

        ammoText.text = ammoInMagazine.ToString();
    }

    public int GetAmmo()
    {
        return ammoInMagazine;
    }

    public void SetAmmoInMagazine(int ammo)
    {
        ammoInMagazine = ammo;
        ammoText.text = ammoInMagazine.ToString();
    }

    public void SetMagazine(GameObject magazineRequired)
    {
        magazine = magazineRequired;
        magazineIsLoaded = true;
    }

    public void MagazineIsNotLoaded()
    {
        magazineIsLoaded = false;
    }

    public void DeactivateGrabMagazineInGun()
    {
        if (magazine != null)
        {
            magazine.GetComponent<Collider>().enabled = false;
        }
    }

    public void ActivateGrabMagazineInGun()
    {
        if (magazine != null)
        {
            magazine.GetComponent<Collider>().enabled = true;
        }
    }

    public abstract void ShootGun();


    public void DecreaseAmmo()
    {
        ammoInMagazine--;
        if(magazineIsLoaded)
            magazine.GetComponent<Magazine>().SetAmmo(ammoInMagazine);
        if (ammoInMagazine < 0)
            ammoInMagazine = 0;
    }


    public void ProcessRecoil()
    {
        if (recoilBody != null)
        {
            //Вторая рука взята
            if (secondHandRigidbody != null)
            {
                recoilBody.AddForce(-transform.forward * recoilForce * .3f, ForceMode.Impulse);
                secondHandRigidbody.AddForce(-transform.forward * recoilForce * .3f, ForceMode.Impulse);
                recoilBody.transform.localRotation = Quaternion.AngleAxis(-1 * recoilForce, Vector3.right);
            }
            else
            {
                recoilBody.AddForce(-transform.forward * recoilForce, ForceMode.Impulse);
                recoilBody.transform.localRotation = Quaternion.AngleAxis(-5 * recoilForce, Vector3.right);
            }
        }
    }


    //
    public void AttemptToShoot(Vector3 recoilOffset, Transform boltTransform, float targetX)
    {
        //Audiosources for sounds
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();

        if (ammoInMagazine > 0)
        {
            if (autoFireMode)
                firingRoutine = StartCoroutine(FiringSequence(audioPoolExample, recoilOffset, boltTransform, targetX));
            else
            {
                StartShoot(audioPoolExample, boltTransform, targetX);
                HitSomething(recoilOffset);
            }
        }
        else
        {
            audioPoolExample.PlayClip(noBulletSound);
        }
        ammoText.text = ammoInMagazine.ToString();
    }

    public void AttemptToShootShotGun(float recoilOffset,float targetX)
    {
        //Audiosources for sounds
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();

        if (ammoInMagazine > 0)
        {
            StartShoot(audioPoolExample, boltTransform, targetX);

            for (int i = 0; i < 9; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-recoilOffset, recoilOffset) / 100f,
                Random.Range(-recoilOffset, recoilOffset) / 100f, Random.Range(-recoilOffset, recoilOffset) / 100f);
                HitSomething(offset);
            }
        }
        else
        {
            audioPoolExample.PlayClip(noBulletSound);
        }
        ammoText.text = ammoInMagazine.ToString();
    }

    private IEnumerator FiringSequence(AudioPoolExample audioPoolExample, Vector3 recoilOffset, Transform boltTransform, float targetX)
    {
        while (gameObject.activeSelf && ammoInMagazine > 0)
        {
            StartShoot(audioPoolExample, boltTransform, targetX);
            HitSomething(recoilOffset);
            yield return wait;

        }
    }



    private void StartShoot(AudioPoolExample audioPoolExample, Transform boltTransform, float targetX)
    {
        DOTween.Sequence()
            .Append(boltTransform.DOLocalMoveX(targetX, 0.04f))
            .Append(boltTransform.DOLocalMoveX(startX, 0.04f));
        DecreaseAmmo();
        ProcessRecoil();
        particleSystem.Play();
        audioPoolExample.PlayClip(shootSound);
        ammoText.text = ammoInMagazine.ToString();

    }


    // NEEDS OFFSET IF MULTIPLE PROJECTILES LIKE SHOTGUN();
    public void HitSomething(Vector3 offset)
    {

        if (Physics.Raycast(muzzlePointTransform.position, muzzlePointTransform.forward + offset, out hit, 100))
        {
            if (hit.collider.gameObject.GetComponent<Collider>())
            { 
                // HIT UNDESTRUCTABLE OBJECT

                if (hit.transform.gameObject.layer == Mathf.Log(undestractableLayer.value, 2))
                {
                    for (int i = 0; i < BulletPoolManager.instance.bulletHoleList.Count; i++)
                    {
                        var bulletHoleElementInstance = BulletPoolManager.instance.bulletHoleList[i];
                        if (bulletHoleElementInstance.activeInHierarchy == false)
                        {

                            bulletHoleElementInstance.SetActive(true);
                            bulletHoleElementInstance.transform.rotation = Quaternion.LookRotation(-hit.normal);
                            bulletHoleElementInstance.transform.position = hit.point + hit.normal.normalized * 0.01f;

                            //CHANGE BEHAVIOR ON IMPACT SOUND
                            PlayImpactSounds(bulletHoleElementInstance);

                            bulletHoleElementInstance.transform.parent = hit.transform;

                            break;
                        }
                        else
                        {
                            if (i == BulletPoolManager.instance.bulletHoleList.Count - 1) // Last Bullet 
                            {
                                GameObject newBulletHole = Instantiate(BulletPoolManager.instance.bulletHolePrefab);
                                newBulletHole.transform.parent = BulletPoolManager.instance.transform;
                                newBulletHole.SetActive(false);

                                //CHANGE BEHAVIOR ON IMPACT SOUND
                                PlayImpactSounds(bulletHoleElementInstance);

                                BulletPoolManager.instance.bulletHoleList.Add(newBulletHole);
                            }
                        }
                    }
                }
                else
                {
                    //TakeDamage
                    if (hit.collider.gameObject.GetComponent<Target>())
                    {
                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            target.TakeDamage(damage);
                        }
                    }
                }
            }
        }
    }

    private void PlayImpactSounds(GameObject bulletHoleElementInstance)
    {
        if (hit.collider.gameObject.GetComponent<MaterialImpactSound>() != null)
        {
            if (hit.collider.gameObject.GetComponent<MaterialImpactSound>().ImpactMaterial.ToString() == "Metal")
            {
                bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = ImpactSoundsManager.instance.MetalImpactSound;
            }

            if (hit.collider.gameObject.GetComponent<MaterialImpactSound>().ImpactMaterial.ToString() == "Flesh")
            {
                bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = ImpactSoundsManager.instance.FleshImpactSound;
            }

            if (hit.collider.gameObject.GetComponent<MaterialImpactSound>().ImpactMaterial.ToString() == "Stone")
            {
                bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = ImpactSoundsManager.instance.StoneImpactSound;
            }

            if (hit.collider.gameObject.GetComponent<MaterialImpactSound>().ImpactMaterial.ToString() == "Wood")
            {
                bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().clip = ImpactSoundsManager.instance.WoodImpactSound;
            }

            bulletHoleElementInstance.transform.gameObject.GetComponent<AudioSource>().Play();
        }
        else { Debug.LogWarning("No MaterialImpactSound Script!"); }
    }


    // XR INTERACTION CHANGES

    public void GrabWeaponForRecoil(SelectEnterEventArgs grabData)
    {

        var hand = grabData.interactorObject;
        var child = hand.transform.GetChild(0);
        recoilBody = child.GetComponent<Rigidbody>();

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

    //public override bool IsSelectableBy(IXRSelectInteractor interactor)
    //{
    //        bool isalreadygrabbed = firstInteractorSelecting != null && !interactor.Equals(firstInteractorSelecting);
    //        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    //}


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor != null && firstInteractorSelecting != null)
        {
            if (SnapToSecondHand)
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation();
            }

            else
            {
                firstInteractorSelecting.transform.rotation = GetTwoHandRotation() * intialRotationOffset;
            }
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Transform attachTransform1 = firstInteractorSelecting.transform;
        Transform attachTransform2 = secondInteractor.transform;

        switch (twoHandRotationType)
        {
            case TwoHandRotationType.None:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position);

            case TwoHandRotationType.First:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, firstInteractorSelecting.transform.up);

            case TwoHandRotationType.Second:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);

            default:
                return Quaternion.LookRotation(attachTransform2.position - attachTransform1.position, secondInteractor.transform.up);
        }

    }

    public void OnSecondHandGrab(SelectEnterEventArgs grabData)
    {
        secondInteractor = grabData.interactorObject;
        var child = secondInteractor.transform.GetChild(0);
        secondHandRigidbody = child.GetComponent<Rigidbody>();
        intialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * secondInteractor.transform.rotation;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        secondHandRigidbody = null;
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        attachIntialRoation = args.interactorObject.transform.localRotation;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        secondInteractor = null;
        args.interactorObject.transform.localRotation = attachIntialRoation;
        base.OnSelectExited(args);
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        if (firingRoutine!=null)
            StopCoroutine(firingRoutine);
        base.OnDeactivated(args);
    }
}
