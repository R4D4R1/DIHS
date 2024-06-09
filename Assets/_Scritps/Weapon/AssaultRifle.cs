using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AssaultRifle : Weapon
{
    [SerializeField] private InputActionProperty releaseMagazine;
    [SerializeField] private LoadMagazineSocketInteractor magazineSocket;
    [SerializeField] private float magazineReleaseTime;

    protected override void Awake()
    {
        releaseMagazine.action.started += Release;
        base.Awake();
    }

    protected override void OnDestroy()
    {
        releaseMagazine.action.started -= Release;
        base.Awake();

    }

    public override void ShootGun()
    {
        AttemptToShoot(Vector3.zero, boltTransform, targetX);
    }

    protected void Release(InputAction.CallbackContext callbackContext)
    {
        if (magazineIsLoaded)
        {
            StartCoroutine(DisableSocket());
        }
    }

    private IEnumerator DisableSocket()
    {
        magazineSocket.GetComponent<Collider>().enabled = false;
        yield return new WaitForSecondsRealtime(0.01f);
        magazineSocket.interactionManager.SelectExit(magazineSocket, magazine.GetComponent<XRGrabInteractable>());
        yield return new WaitForSecondsRealtime(magazineReleaseTime);
        magazineSocket.GetComponent<Collider>().enabled = true;
    }
}
