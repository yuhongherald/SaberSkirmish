using UnityEngine;

/// <summary>
/// A visual instructional on how to swivel phone, demostrated using a virtual phone model.
/// </summary>
public class VirtualPhone : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Vector3 rotation1;
    [SerializeField]
    private Vector3 rotation2;
    [SerializeField]
    private int totalFrames = 120;

    private int animationFrame = 0;

    public bool IsLockingControls()
    {
        return (player != null);
    }

    private void Update()
    {
        UpdateVirtualPhonePosition();
        ControlPlayerWeapon();
    }

    private void UpdateVirtualPhonePosition()
    {
        animationFrame = (animationFrame + 1) % totalFrames;
        Vector3 eulerRotation = Vector3.Lerp(rotation2, rotation1,
            (Mathf.Sin((float)animationFrame / (float)totalFrames * Mathf.PI * 2.0f) + 1.0f) / 2);
        gameObject.transform.localEulerAngles = eulerRotation;
    }

    private void ControlPlayerWeapon()
    {
        if (player != null)
        {
            player.GetWeaponPoint().transform.localRotation = gameObject.transform.localRotation;
        }
    }
}
