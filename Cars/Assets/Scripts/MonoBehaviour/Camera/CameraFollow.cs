
using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    public Transform target;
    public float distance = 5f;
    public Vector3 cameraOffset;
    [SerializeField, Min(0f)]
    float targetRadius = 1f;
    [SerializeField, Range(0f, 1f)]
    float radiusCenter = 0.5f;
    public bool flipCamera = false;
    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;
    [SerializeField, Range(-89f, 89f)]
    float minVerticalAngle = -30f, maxVerticalAngle = 60f;
    [SerializeField, Min(0f)]
    float alignDelay = 1f;
    [SerializeField, Range(0f, 90f)]
    float alignSmoothRange = 45f;

    Vector2 orbitAngles = new Vector2(20f, 0f);
    Vector3 targetPoint, previousTargetPoint;
    float lastManualRotationTime;

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    private void Awake()
    {
        targetPoint = target.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
    }

    private void LateUpdate()
    {
        UpdateTarget();
        Quaternion lookRotation;

        if (ManualRotation() || AutomaticRotation())
        {
            ConstraintAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else
            lookRotation = transform.localRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = targetPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateTarget()
    {
        previousTargetPoint = targetPoint;
        Vector3 updatedTarget = target.position;

        if (targetRadius > 0f)
        {
            float distance = Vector3.Distance(updatedTarget, targetPoint);
            float t = 1f;
            if (distance > 0.01f && radiusCenter > 0f)
            {
                t = Mathf.Pow(1f - radiusCenter, Time.unscaledDeltaTime);
            }

            if (distance > targetRadius)
            {
                t = Mathf.Min(t, radiusCenter / distance);
            }
            targetPoint = Vector3.Lerp(targetPoint, updatedTarget, t);
        }
        else
            targetPoint = updatedTarget;

        targetPoint += cameraOffset;
    }

    bool ManualRotation()
    {
        Vector2 input;
        if (flipCamera)
        {
            input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }
        else
        {
            input = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    void ConstraintAngles()
    {
        orbitAngles.x =
            Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f)
            orbitAngles.y += 360f;
        else if (orbitAngles.y >= 360f)
            orbitAngles.y -= 360f;
    }

    bool AutomaticRotation()
    {
        if (Time.unscaledTime - lastManualRotationTime < alignDelay)
            return false;

        Vector2 movement = new Vector2(targetPoint.x - previousTargetPoint.x, targetPoint.y - previousTargetPoint.y);
        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.00001f)
            return false;

        float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        if(deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        else if(180f - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / alignSmoothRange;
        }
        orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
        return true;
    }

    static float GetAngle( Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }
}

