using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;


public class NetworkGun : NetworkBehaviour
{
    protected Weapon weapon;
    protected LoadMagazineSocketInteractor magazineSlot;
    [SerializeField] protected int startAmmo;
    [SerializeField] private InputActionProperty reloadGun;


    private void Awake()
    {
        weapon = GetComponent<Weapon>();
        magazineSlot = GetComponentInChildren<LoadMagazineSocketInteractor>();
    }


    private void Start()
    {
        weapon.activated.AddListener(AttemptToShootNetworkGun);
        reloadGun.action.started += ReloadWeaponNetwork;

        //magazineSlot.selectEntered.AddListener(LoadMagazineNetwork);
    }

    void OnDestroy()
    {
        weapon.activated.RemoveListener(AttemptToShootNetworkGun);
        reloadGun.action.started -= ReloadWeaponNetwork;

        //magazineSlot.selectEntered.RemoveListener(LoadMagazineNetwork);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isClient)
            {
                AttemptToShootNetworkGun(null);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isClient)
            {
                CmdReloadWeaponNetwork();
            }
        }
    }



    [Command(requiresAuthority = false)]
    public void AttemptToShootNetworkGun(ActivateEventArgs arg)
    {
        RpcAttemptToShootNetworkGun();
    }

    [ClientRpc]
    public void RpcAttemptToShootNetworkGun()
    {
        weapon.ShootGun();

    }





    protected void ReloadWeaponNetwork(InputAction.CallbackContext context)
    {
        if (isServer)
            RpcReloadWeaponNetwork();
        else
            CmdReloadWeaponNetwork();
    }

    [ClientRpc]
    void RpcReloadWeaponNetwork()
    {
        weapon.ReloadGun();
    }

    [Command(requiresAuthority = false)]
    public void CmdReloadWeaponNetwork()
    {
        RpcReloadWeaponNetwork();
    }


    //public void LoadMagazineNetwork(SelectEnterEventArgs magazineData)
    //{
    //    if (isServer)
    //        RpcLoadMagazineNetwork(magazineData);
    //    else
    //        CmdLoadMagazineNetwork(magazineData);
    //}

    //[ClientRpc]
    //void RpcLoadMagazineNetwork(SelectEnterEventArgs magazineData)
    //{
    //    weapon.SetAmmoInMagazine(magazineData.interactableObject.transform.gameObject.GetComponent<Magazine>().GetAmmo());
    //    magazineSlot.GetComponent<AudioSource>().clip = weapon.reloadingSound;
    //    magazineSlot.GetComponent<AudioSource>().Play();
    //    weapon.SetMagazine(magazineData.interactableObject.transform.gameObject);
    //}

    //[Command(requiresAuthority = false)]
    //public void CmdLoadMagazineNetwork(SelectEnterEventArgs magazineData)
    //{
    //    RpcLoadMagazineNetwork(magazineData);
    //}
}
