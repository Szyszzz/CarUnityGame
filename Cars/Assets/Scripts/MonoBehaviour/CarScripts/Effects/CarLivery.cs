using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLivery : MonoBehaviour
{
    public GameObject CarGFX;
    public Material BaseMaterial;

    public Color ChassisColor;
    public Color WindowsColor;
    public Texture2D CarDecals;
    public bool UseDecal;

    private Material carMaterial;

    void Start()
    {
        carMaterial = new Material(BaseMaterial);
        carMaterial.SetColor("_CarColor", ChassisColor);
        carMaterial.SetColor("_WindowsColor", WindowsColor);
        carMaterial.SetTexture("_DecalTexture", CarDecals);
        if (UseDecal)
            carMaterial.SetFloat("_DecalBool", 1);
        else
            carMaterial.SetFloat("_DecalBool", 0);

        Material[] mat = CarGFX.GetComponent<Renderer>().materials;
        mat[0] = carMaterial;
        CarGFX.GetComponent<Renderer>().materials = mat;
    }

    void Update()
    {
        
    }
}
