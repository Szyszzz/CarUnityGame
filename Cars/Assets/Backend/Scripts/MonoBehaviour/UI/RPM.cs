using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RPM : MonoBehaviour
{
    public CarMovement1 CarScript;
    public int dialMaxOffset;

    private float rpm;

    void Start()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (CarScript == null)
            return;

        if (CarScript.engine.maxRPM <= 0 || CarScript.motorRPM <= 0)
            return;

        rpm = (CarScript.motorRPM / CarScript.engine.maxRPM) * dialMaxOffset;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(rpm, 0, 0);
    }
}
