using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LoadMagazineSocketInteractor : XRSocketInteractor
{
   protected override void OnSelectEntering(SelectEnterEventArgs magazineData)
   {
        transform.parent.GetComponent<Weapon>().SetAmmoInMagazine(magazineData.interactableObject.transform.gameObject.GetComponent<Magazine>().GetAmmo());
        GetComponent<AudioSource>().clip = transform.parent.GetComponent<Weapon>().reloadingSound;
        GetComponent<AudioSource>().Play();
        transform.parent.GetComponent<Weapon>().SetMagazine(magazineData.interactableObject.transform.gameObject);
        base.OnSelectEntering(magazineData);
   }

    protected override void OnSelectExited(SelectExitEventArgs magazineData)
    {
        GetComponent<AudioSource>().clip = transform.parent.GetComponent<Weapon>().reloadingSound;
        GetComponent<AudioSource>().Play();
        transform.parent.GetComponent<Weapon>().SetAmmoInMagazine(0);
        transform.parent.GetComponent<Weapon>().MagazineIsNotLoaded();
        base.OnSelectExited(magazineData);
    }
    protected override void OnDisable()
    {
        GetComponent<AudioSource>().clip = transform.parent.GetComponent<Weapon>().reloadingSound;
        GetComponent<AudioSource>().Play();
        transform.parent.GetComponent<Weapon>().SetAmmoInMagazine(0);
        transform.parent.GetComponent<Weapon>().MagazineIsNotLoaded();
        base.OnDisable();
    }

}
