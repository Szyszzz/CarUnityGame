using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gear : MonoBehaviour
{
    public Sprite[] GearGFX;
    public CarMovement1 CarScript;

    private int gearError = 0;

    // Update is called once per frame
    void Update()
    {
        if(CarScript == null | GearGFX.Length < CarScript.currentGear)
        {
            if (GearGFX.Length < 2)
            {
                return;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = GearGFX[gearError];
                gearError = gearError++ % GearGFX.Length;
                return;
            }
        }
        gameObject.GetComponent<Image>().sprite = GearGFX[CarScript.currentGear];
    }
}
