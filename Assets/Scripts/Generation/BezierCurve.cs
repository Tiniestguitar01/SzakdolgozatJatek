using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BezierCurve
{
    public Vector3 startPoint;
    public Vector3 startDirectionPoint;
    public Vector3 endDirectionPoint;
    public Vector3 endPoint;

    public BezierCurve(Vector3 startPoint, Vector3 startDirectionPoint, Vector3 endDirectionPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.startDirectionPoint = startDirectionPoint;
        this.endDirectionPoint = endDirectionPoint;
        this.endPoint = endPoint;
    }

    public Vector3 ReturnPosition(float time)
    {
        Vector3 p1 = Mathf.Pow(1 - time, 3) * startPoint;
        Vector3 p2 = 3 * Mathf.Pow(1 - time, 2) * time * startDirectionPoint;
        Vector3 p3 = 3 * (1-time) * Mathf.Pow(time, 2) * endDirectionPoint;
        Vector3 p4 = Mathf.Pow(time, 3) * endPoint;
        return p1 + p2 + p3 + p4;
    }

    public Vector3 ReturnDerivative(float time)
    {
        Vector3 p1 = (-3 * Mathf.Pow( time, 2) + 6 * time - 3) * startPoint;
        Vector3 p2 = (9 * Mathf.Pow(time, 2) - 12 * time + 3) * startDirectionPoint;
        Vector3 p3 = (-9 * Mathf.Pow(time, 2) + 6 * time) * endDirectionPoint;
        Vector3 p4 = 3 * Mathf.Pow(time, 2) * endPoint;
        return p1 + p2 + p3 + p4;
    }

    public Vector3 ReturnPositionBasedOnPosition(float time,Vector3 position)
    {
        return ReturnPosition(time) + Quaternion.LookRotation(ReturnDerivative(time).normalized) * position;
    }

    public Vector3 ReturnPositionBasedOnVector(float time, Vector3 position)
    {
        return Quaternion.LookRotation(ReturnDerivative(time).normalized) * position;
    }

    public Quaternion GetRotation(float time)
    {
        return Quaternion.LookRotation(ReturnDerivative(time).normalized);
    }
}
