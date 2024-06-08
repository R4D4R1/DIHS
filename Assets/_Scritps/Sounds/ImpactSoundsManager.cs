using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSoundsManager : MonoBehaviour
{
    public static ImpactSoundsManager instance = null;

    public AudioClip MetalImpactSound;
    public AudioClip StoneImpactSound;
    public AudioClip FleshImpactSound;
    public AudioClip WoodImpactSound;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }


}
