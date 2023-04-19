using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    public GameObject[] disable;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Alpha1) && Input.GetKey(KeyCode.Alpha3) && Input.GetKey(KeyCode.Alpha7))
        {
            foreach(GameObject dis in disable)
            {
                dis.SetActive(false);
            }

            Debug.LogWarning("CheatCode");
        }       
    }
}
