using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#region FrictionClass - Class used to create WheelFrictionCurve to be passed on to the WheelColiders

[System.Serializable]
public class Friction
{
    public float extremumSlip = 1f;
    public float extremumValue = 1f;
    public float asymptoteSlip = 0.8f;
    public float asymptoteValue = 0.5f;
    public float stiffness = 1f;

    public WheelFrictionCurve ValuesToCurve()
    {
        WheelFrictionCurve curve = new WheelFrictionCurve();
        curve.extremumSlip = extremumSlip;
        curve.extremumValue = extremumValue;
        curve.asymptoteSlip = asymptoteSlip;
        curve.asymptoteValue = asymptoteValue;
        curve.stiffness = stiffness;
        return curve;
    }
}
#endregion

#region AxleClass - Class used to be able to change stats per vehicle Axle (Front, Rear etc.) in inspector, rather than to change them manually for each wheel
[System.Serializable]
public class Axle
{
    public WheelCollider lWheel;
    public WheelCollider rWheel;
    public GameObject wheelPrefab;
    public bool driveAxle;
    public bool steeringAxle;
    public bool hasHandbrake;
    public float mass = 20f;
    public float radius = 0.25f;
    public float wheelDampingRate = 0.5f;
    public float suspensionDistance = 0.02f;
    public float forceAppPointDistance = 0f;
    [Space(1)]
    public Vector3 center = Vector3.zero;
    [Space(1)]
    [Header("Suspension Spring")]
    public float spring = 3500f;
    public float damper = 2000f;
    public float targetPosition = 0f;
    [Space(5)]
    public Friction forwardFriction;
    [Space(5)]
    public Friction sidewaysFriction;
    [Space(5)]
    public Friction handbrakeFriction;

    [HideInInspector]
    public List<WheelCollider> wheels;

    public void FillWheelsList() => wheels = new List<WheelCollider>() {lWheel, rWheel };

    public void UpdateWheelStats(WheelCollider wheel)
    {
        JointSpring jointSpring = new JointSpring();
        jointSpring.spring = spring;
        jointSpring.damper = damper;
        jointSpring.targetPosition = targetPosition;
        wheel.mass = mass;
        wheel.radius = radius;
        wheel.wheelDampingRate = wheelDampingRate;
        wheel.suspensionDistance = suspensionDistance;
        wheel.forceAppPointDistance = forceAppPointDistance;
        wheel.center = center;
        wheel.suspensionSpring = jointSpring;
        wheel.forwardFriction = forwardFriction.ValuesToCurve();
        wheel.sidewaysFriction = sidewaysFriction.ValuesToCurve();
    }

    public float GetAxleRPM() => (rWheel.rpm + lWheel.rpm) / 2;

    public WheelFrictionCurve HandbrakeFrictionCurve() => handbrakeFriction.ValuesToCurve();

    public WheelFrictionCurve SidewaysFrictionCurve() => sidewaysFriction.ValuesToCurve();

    public WheelFrictionCurve ForwardFrictionCurve() => forwardFriction.ValuesToCurve();

}
#endregion

public class CarMovement1 : MonoBehaviour
{
    #region Inspector Info

    public List<Axle> axles;
    public float maxSteeringAngle;

    public int driveAxles;
    public Wheels wheelObject;


    [Space(5)]
    [Header("Engine")]
    public Engines engine;
    [Space(5)]
    public float currentTorque;
    public float motorRPM;

    [Space(5)]
    [Header("Gear Ratios")]
    public float reverse;
    public float[] driveGears;
    public float finalDrive;

    [Space(5)]
    public int currentGear = 1;
    public bool isCarInReverse = false;

    [Space(5)]
    [Header("Pedals and Handbrake")]
    public float accelerator;
    [Range(0.01f, 3f)] public float acceleratorSmoothness = 1f;
    public float brake;
    public float brakeForce;
    [Range(1f, 3f)] public float brakeSmoothness = 1f;
    public bool isHandbrakeOn;
    [Range(0f,2f)] public float handbrakeForceMultiplier = 1f;

    private float[] gears;
    private float lastMotorRPM;
    private float lastWheelRPM;

    #endregion

    #region validationMethods - Methods that are called during Validate. Most of them update stats, generate GFX and fix GFX rotations

    private void ValidateGearsArray()
    {
        List<float> tempGears = new List<float>();
        tempGears.Add(reverse);
        tempGears.Add(0);
        foreach (float gear in driveGears)
            tempGears.Add(gear);

        gears = tempGears.ToArray();
    }

    private void UpdateWheelListsAndStats()
    {
        // Populate Wheels list with RWheel and LWheel
        // Change the values to the one from script
        // Make the wheel coliders on the objects not editable
        foreach (Axle axle in axles)
        {
            axle.FillWheelsList();
            axle.UpdateWheelStats(axle.lWheel);
            axle.UpdateWheelStats(axle.rWheel);

            foreach (WheelCollider wheel in axle.wheels)
            {
                wheel.hideFlags = HideFlags.NotEditable;
            }
        }
    }

    private void GenerateWheelGFX()
    {
        foreach (Axle axle in axles)
        {
            Transform lGFX = axle.lWheel.transform.Find("Wheel_GFX");
            Transform rGFX = axle.rWheel.transform.Find("Wheel_GFX");

            if (axle.wheelPrefab == null)
                axle.wheelPrefab = wheelObject.LoadWheel();

            if (lGFX == null)
                lGFX = Instantiate(axle.wheelPrefab, axle.lWheel.transform).transform;

            if (rGFX == null)
                rGFX = Instantiate(axle.wheelPrefab, axle.rWheel.transform).transform;

            lGFX.name = "Wheel_GFX";
            rGFX.name = "Wheel_GFX";

            Vector3 flipRightWheel = new Vector3(-Mathf.Abs(rGFX.localScale.x), -Mathf.Abs(rGFX.localScale.y), rGFX.localScale.z);
            rGFX.transform.localScale = flipRightWheel;
        }
    }

    private void ValidateHowManyDriveAxles()
    {
        driveAxles = 0;
        foreach (Axle axle in axles)
            if (axle.driveAxle)
                driveAxles++;
    }

    #endregion

    private void CorrectWheelRotations()
    {
        foreach (Axle axle in axles)
        {
            foreach (WheelCollider wheel in axle.wheels)
            {
                Transform wheelGFX = wheel.transform.GetChild(0);
                Vector3 pos;
                Quaternion rot;

                wheel.GetWorldPose(out pos, out rot);

                wheelGFX.transform.position = pos;
                wheelGFX.transform.rotation = rot;
            }
        }
    }

    private float CalculateCurrentRPM()
    {
        lastMotorRPM = motorRPM;
        float wheelRPM = 0;
        foreach (Axle axle in axles)
            if (axle.driveAxle)
            {
                wheelRPM += Mathf.Abs(axle.GetAxleRPM());
            }

        wheelRPM = wheelRPM / driveAxles;

        float newRPM = Mathf.Clamp(wheelRPM * finalDrive * gears[currentGear], engine.idleRPM, engine.maxRPM);
        return Mathf.Lerp(lastMotorRPM, newRPM, Time.deltaTime);
    }

    private void ChangeGears()
    {
        // gears go up
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (currentGear >= gears.Length - 1)
                return;
            else
            {
                if (currentGear == 0)
                    isCarInReverse = false;
                currentGear++;
            }
        }
        // gears go down
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentGear == 0)
                return;
            else
            {
                currentGear--;
                if(currentGear == 0)
                    isCarInReverse = true;
            }
        }
    }

    private float CurrentMotorTorque(float motorRPM)
    {
        switch(isCarInReverse)
        {
            case true:
                return -(engine.torqueCurve.Evaluate(motorRPM) * gears[currentGear] * finalDrive * accelerator);
            case false:
                return engine.torqueCurve.Evaluate(motorRPM) * gears[currentGear] * finalDrive * accelerator;
        }
    }

    public float CalculateEngineBrakingTorque()
    {
        if (accelerator <= 0)
        {
            float wheelRPM = 0;
            foreach (Axle axle in axles)
                if (axle.driveAxle)
                    wheelRPM += Mathf.Abs(axle.GetAxleRPM());
            wheelRPM = wheelRPM / driveAxles;

            return engine.torqueCurve.Evaluate(wheelRPM * gears[currentGear] * finalDrive * engine.engineBrakingValue);
        }
        else
            return 0f;
    }

    private void CalculateAccelerator()
    {
        float accel;
        if (Input.GetAxisRaw("Vertical") > 0)
            accel = accelerator + Input.GetAxisRaw("Vertical") * Time.deltaTime * acceleratorSmoothness;
        else
            accel = accelerator * Time.deltaTime * acceleratorSmoothness;

        if (accel <= 0.0001f)
            accel = 0f;

        accelerator = Mathf.Clamp(accel, 0f, 1f);
    }

    private void CalculateBrakePedal()
    {
        float brakePedal;
        if(Input.GetAxisRaw("Vertical") < 0)
            brakePedal = brake + -Input.GetAxisRaw("Vertical") * Time.deltaTime * brakeSmoothness;
        else
            brakePedal = brake - Time.deltaTime * brakeSmoothness;

        brake = Mathf.Clamp(brakePedal, 0f, 1f);
    }

    private void HandbrakeInput()
    {
        if(Input.GetKey(KeyCode.Space))
            isHandbrakeOn = true;
        else
            isHandbrakeOn = false;
    }

    private float HandbrakeForceApplied(float motorTorque)
    {
        float handbrakeForce;
        if (isHandbrakeOn)
        {
            handbrakeForce = motorTorque * handbrakeForceMultiplier;
            foreach(Axle axle in axles)
            {
                if (axle.hasHandbrake)
                    foreach (WheelCollider wheel in axle.wheels)
                    {
                        wheel.sidewaysFriction = axle.HandbrakeFrictionCurve();
                        wheel.forwardFriction = axle.HandbrakeFrictionCurve();
                    }
            }
        }
        else
        {
            handbrakeForce = 0f;
            foreach (Axle axle in axles)
            {
                if (axle.hasHandbrake)
                    foreach (WheelCollider wheel in axle.wheels)
                    {
                        wheel.sidewaysFriction = axle.SidewaysFrictionCurve();
                        wheel.forwardFriction = axle.ForwardFrictionCurve();
                    }
            }
        }

        return handbrakeForce;

    }

    private float CalculateFinalBrakingForce(Axle axle)
    {
        float brakingForce = 0f;

        switch(axle.hasHandbrake)
        {
            case true:
                brakingForce += HandbrakeForceApplied(currentTorque);
                break;
        }
        switch(axle.driveAxle)
        {
            case true:
                brakingForce += CalculateEngineBrakingTorque();
                break;
        }
        brakingForce += (brakeForce * brake);

        return brakingForce / driveAxles;
    }

    private void Start()
    {
        UpdateWheelListsAndStats();
        GenerateWheelGFX();
        ValidateGearsArray();
        ValidateHowManyDriveAxles();
    }

    private void FixedUpdate()
    {
        CorrectWheelRotations();

        float torquePerWheel = currentTorque / (driveAxles * 2);
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        Debug.Log($"G: {currentGear} Engine RPM: {motorRPM} Accel: {accelerator}");

        foreach (Axle axle in axles)
        {
            float axleBrakingForce = CalculateFinalBrakingForce(axle);
            foreach (WheelCollider wheel in axle.wheels)
            {
                if (axle.steeringAxle)
                    wheel.steerAngle = steering;
                if (axle.driveAxle)
                {
                    wheel.motorTorque = torquePerWheel;
                }
                wheel.brakeTorque = axleBrakingForce / 2;
            }
        }
    }

    void Update()
    {
        ChangeGears();
        CalculateAccelerator();
        CalculateBrakePedal();
        HandbrakeInput();

        motorRPM = CalculateCurrentRPM();
        currentTorque = CurrentMotorTorque(motorRPM);
    }
}
