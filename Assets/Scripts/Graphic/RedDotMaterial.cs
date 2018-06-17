using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDotMaterial : MonoBehaviour {
    public Material redDotMaterialPrefab;
    // More materials for later there
    [Tooltip("Temporarily doesn't do shit")]
    public HoloType holoType;
	void Start () {
        SetMaterial(redDotMaterialPrefab);
    }
    public void SetMaterial(Material sourceMaterial)
    {
        GetComponent<Renderer>().material = new Material(sourceMaterial);
    }
}

public enum HoloType
{
    RedDot, GreenDot, RedHolo, GreenHolo, RedArrow
}