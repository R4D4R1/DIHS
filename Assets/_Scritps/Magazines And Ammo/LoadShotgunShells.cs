//using System.Collections;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Transformers;

//public class LoadShotgunShells : XRSocketInteractor
//{
//    [SerializeField] private UnityEvent noSpaceForAmmo;

//    protected override void OnSelectEntering(SelectEnterEventArgs ShotgunShell)
//    {
//        var shotgun = transform.parent.GetComponent<ShotGun>();
//        if (shotgun.GetAmmo()<shotgun.GetMaxAmmo())
//        {
//            shotgun.SetAmmoInMagazine(shotgun.AddAmmo());
//            GetComponent<AudioSource>().clip = transform.parent.GetComponent<Weapon>().reloadingSound;
//            GetComponent<AudioSource>().Play();

//            ShotgunShell.interactableObject.transform.gameObject.SetActive(false);

//            // «¿–ﬂƒ»À» œŒ—À≈ƒÕ»… œ¿“–ŒÕ
//            if (shotgun.GetAmmo() == shotgun.GetMaxAmmo())
//            {
//                //Debug.Log("Last bullet");
//                noSpaceForAmmo.Invoke();
//            }

//            //StartCoroutine(DestroyAmmo(ShotgunShell.interactableObject.transform.gameObject));
//        }



//        base.OnSelectEntering(ShotgunShell);
//    }

//    private IEnumerator DestroyAmmo(GameObject gameObject)
//    {
//        yield return new WaitForSecondsRealtime(3f);
//        Destroy(gameObject);
//    }

//}
