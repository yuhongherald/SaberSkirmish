using System;
using UnityEngine;

/// <summary>
/// A collection of methods to convert and handle different data types.
/// </summary>
public class Util
{
    private const char VECTOR_SEPARATOR = ',';

    public static Func<T2> Curry2<T1, T2>(Func<T1, T2> function, T1 arg)
    {
        return () => function(arg);
    }

    public static string Vector3ToString(Vector3 vector)
    {
        string result = string.Join(VECTOR_SEPARATOR.ToString(),
            new string[3]{vector.x.ToString(),
                          vector.y.ToString(), vector.z.ToString()});
        return result;
    }

    public static Vector3 StringToVector3(string sVector)
    {
        string[] sArray = sVector.Split(VECTOR_SEPARATOR);
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));
        return result;
    }

    public static string QuaternionToString(Quaternion quaternion)
    {
        string result = string.Join(VECTOR_SEPARATOR.ToString(),
            new string[4]{quaternion.w.ToString(), quaternion.x.ToString(),
                          quaternion.y.ToString(), quaternion.z.ToString()});
        return result;
    }

    public static Quaternion StringToQuaternion(string sQuaternion)
    {
        string[] sArray = sQuaternion.Split(VECTOR_SEPARATOR);
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));
        return result;
    }

    public static string BoolToString(bool operand)
    {
        return operand ? "1" : "0";
    }

    public static bool StringToBool(string operand)
    {
        return operand.Equals("1");
    }

    public static Vector3[] ComputeSpeed(Vector3[] controlPoints, float t)
    {
        if (controlPoints.Length <= 1) return new Vector3[0];
        else if (controlPoints.Length == 2) return controlPoints;

        Vector3[] newControlPoints = new Vector3[controlPoints.Length - 1];
        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            newControlPoints[i] = Vector3.Lerp(controlPoints[i], controlPoints[i + 1], t);
        }
        return ComputeSpeed(newControlPoints, t);
    }

    public static Vector3 ComputeBezier(Vector3[] controlPoints, float t)
    {
        if (controlPoints.Length == 0) return Vector3.zero;
        else if (controlPoints.Length == 1) return controlPoints[0];
        else if (controlPoints.Length > 2) return ComputeBezier(ComputeSpeed(controlPoints, t), t);
        return Vector3.Lerp(controlPoints[0], controlPoints[1], t);
    }

    public static Vector3 ComputePoint(Vector3 velocity, Vector3 basePoint, bool left, float width)
    {
        Vector3 tangent = Vector3.Cross(velocity, -Vector3.forward).normalized;
        Vector3 displacement = tangent * width / 2;
        return basePoint + ((left) ? (-displacement) : displacement);
    }

    public static float Pyramid(float max, float current)
    {
        return (max - Mathf.Abs(max - current));
    }

    public static float Lerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }
}
