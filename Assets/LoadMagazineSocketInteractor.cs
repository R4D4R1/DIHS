using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LoadMagazineSocketInteractor : XRSocketInteractor
{
   protected override void OnSelectEntering(SelectEnterEventArgs magazineData)
   {
        transform.parent.GetComponent<Gun>().SetAmmoInMagazine(magazineData.interactableObject.transform.gameObject.GetComponent<Magazine>().GetAmmo());
         
        base.OnSelectEntering(magazineData);
   }

    protected override void OnSelectExited(SelectExitEventArgs magazineData)
    {
        magazineData.interactableObject.transform.gameObject.GetComponent<Magazine>().SetAmmo(transform.parent.GetComponent<Gun>().GetAmmo());
        transform.parent.GetComponent<Gun>().SetAmmoInMagazine(0);
        base.OnSelectExited(magazineData);
    }
}
