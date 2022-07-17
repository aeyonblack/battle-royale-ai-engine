using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistics
{
    public static float? RotateLaunchPoint(Transform launchPoint, Vector3 target, float launchForce, bool lowAngle = false)
    {
        float? angle = CalculateAngle(lowAngle, launchPoint, target, launchForce);
        if (angle != null)
        {
            launchPoint.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    private static float? CalculateAngle(bool low, Transform launchPoint, Vector3 target, float launchForce)
    {
        Vector3 targetDirection = target - launchPoint.position;
        float y = targetDirection.y;
        targetDirection.y = 0;
        float x = targetDirection.magnitude;
        float gravity = Physics.gravity.magnitude;
        float squareSpeed = launchForce * launchForce;
        float underSquareRoot = (squareSpeed * squareSpeed) - gravity * (gravity * x * x + 2 * y * squareSpeed);

        if (underSquareRoot >= 0)
        {
            float root = Mathf.Sqrt(underSquareRoot);
            float highAngle = squareSpeed + root;
            float lowAngle = squareSpeed - root;

            if (low)
            {
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            }
            else
            {
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
            }
        }
        return null;
    }
}
