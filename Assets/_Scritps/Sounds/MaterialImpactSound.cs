using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialImpactSound : MonoBehaviour
{
    [Serializable]
    public enum ImpactMaterialEnum
    {
        Stone,
        Wood,
        Metal,
        Flesh
    };

    [field: SerializeField]
    public ImpactMaterialEnum ImpactMaterial
    {
        get;
        private set;
    }
}
