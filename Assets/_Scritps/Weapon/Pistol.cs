using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : Weapon
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
        Debug.Log("Attempt Relaod");
        if (magazineIsLoaded)
        {
            StartCoroutine(DisableSocket());
        }
    }

    private IEnumerator DisableSocket()
    {
        magazineSocket.GetComponent<Collider>().enabled = false;
        yield return new WaitForSecondsRealtime(0.05f);
        magazineSocket.interactionManager.SelectExit(magazineSocket, magazine.GetComponent<XRGrabInteractable>());
        yield return new WaitForSecondsRealtime(magazineReleaseTime);
        magazineSocket.GetComponent<Collider>().enabled = true;
    }
}
