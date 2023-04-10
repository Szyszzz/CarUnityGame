using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    public GameObject CarGFX;
    public Material BaseLightMaterial;
    public Texture2D LightTexture;
    public Texture2D LightsMask;
    [Range(0f, 1f)] public float BlinkerInterval = 0.5f;

    private Material lightMaterial;
    private bool markersOn = false;
    private bool headlightsOn = false;
    private bool beamsOn = false;
    private byte frontState = 0;
    private float signalTimer = 0;
    private byte signalState = 0;
    private bool blink = true;
    

    private void Brakelights(bool isOn)
    {
        if (isOn)
            lightMaterial.SetFloat("_Brakelights", 1);
        else
            lightMaterial.SetFloat("_Brakelights", 0);
    }

    private void Markers()
    {
        if (markersOn)
            lightMaterial.SetFloat("_Markers", 1);
        else
            lightMaterial.SetFloat("_Markers", 0);
    }

    private void Headlights()
    {
        if (headlightsOn)
            lightMaterial.SetFloat("_Headlights", 1);
        else
            lightMaterial.SetFloat("_Headlights", 0);

    }

    private void Beams()
    {
        if (beamsOn)
            lightMaterial.SetFloat("_Beams", 1);
        else
            lightMaterial.SetFloat("_Beams", 0);
    }

    private bool Blink()
    {
        if (signalTimer < BlinkerInterval)
        {
            signalTimer += Time.deltaTime;     
        }
        else
        {
            signalTimer = 0;
            blink = !blink;
        }
        return blink;
    }
    // Signal states: 0 - OFF, 1 - Left, 2 - Right, 3 - Hazards
    private void SignalState()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            switch(signalState)
            {
                case 3:
                    signalState = 0;
                    break;
                default:
                    signalState = 3;
                    break;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.E))
        {
            signalState = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (signalState)
            {
                case 1:
                    signalState = 0;
                    break;
                case 2:
                    signalState = 0;
                    break;
                case 3:
                    break;
                default:
                    signalState = 1;
                    break;
            }
            return;
        }
        else
        {
            switch (signalState)
            {
                case 1:
                    signalState = 0;
                    break;
                case 2:
                    signalState = 0;
                    break;
                case 3:
                    break;
                default:
                    signalState = 2;
                    break;
            }
            return;
        }
    }

    private void Signal()
    {
        switch(signalState)
        {
            case 1:
                if (Blink())
                    lightMaterial.SetFloat("_Left", 1);
                else
                    lightMaterial.SetFloat("_Left", 0);
                break;

            case 2:
                if (Blink())
                    lightMaterial.SetFloat("_Right", 1);
                else
                    lightMaterial.SetFloat("_Right", 0);
                break;

            case 3:
                if (Blink())
                    lightMaterial.SetFloat("_Hazards", 1);
                else
                    lightMaterial.SetFloat("_Hazards", 0);
                break;
            default:
                lightMaterial.SetFloat("_Left", 0);
                lightMaterial.SetFloat("_Right", 0);
                lightMaterial.SetFloat("_Hazards", 0);
                signalTimer = 0;
                blink = true;
                break;
        }
    }

    private void Start()
    {
        lightMaterial = new Material(BaseLightMaterial);
        lightMaterial.SetTexture("_LightTextures", LightTexture);
        lightMaterial.SetTexture("_LightMask", LightsMask);

        Material[] mat = CarGFX.GetComponent<Renderer>().materials;
        mat[1] = lightMaterial;
        CarGFX.GetComponent<Renderer>().materials = mat;
    }

    private void Update()
    {
        Brakelights(Input.GetKey(KeyCode.S));

        if(Input.GetKeyDown(KeyCode.L))
        {
            frontState++;
            if (frontState >= 4)
                frontState = 0;
            switch(frontState)
            {
                case 1:
                    markersOn = true;
                    break;
                case 2:
                    headlightsOn = true;
                    break;
                case 3:
                    beamsOn = true;
                    break;
                default:
                    markersOn = false;
                    headlightsOn = false;
                    beamsOn = false;
                    break;
            }
            Markers();
            Headlights();
            Beams();
        }

        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            SignalState();

        Signal();
    }
}
