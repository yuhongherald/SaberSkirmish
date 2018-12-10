using UnityEngine;

/// <summary>
/// A collection of methods to manipulate 3D scene objects.
/// </summary>
public class ShapeUtil
{
    public static void SetNormal(GameObject gameObject, Vector3 newNormal, Vector3 oldNormal)
    {
        if (newNormal.magnitude == 0)
        {
            return;
        }
        gameObject.transform.localRotation = Quaternion.FromToRotation(oldNormal, newNormal);
    }
}
