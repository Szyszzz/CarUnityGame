using UnityEngine;
using Cinemachine;

public class FreeLookSwitch : MonoBehaviour
{
    private void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetKey(KeyCode.V))
                return Input.GetAxis("Mouse X");
            else
                return 0;
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetKey(KeyCode.V))
                return Input.GetAxis("Mouse Y");
            else
                return 0;
        }
        return Input.GetAxis(axisName);
    }
}
