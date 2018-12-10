using UnityEngine;

/// <summary>
/// An instructional text composed of an <see cref="Instruction"/>,
/// <see cref="DirectionalArrow"/>, and <see cref="VirtualPhone"/>.
/// </summary>
public class ArcadeInstruction : MonoBehaviour
{
    [SerializeField]
    private Instruction instruction;
    [SerializeField]
    private DirectionalArrow arrow;
    [SerializeField]
    private VirtualPhone phone;

    /// <summary>
    /// Sets the animation state.
    /// </summary>
    /// <param name="active">Animation state to be set.</param>
    public void SetActive(bool active)
    {
        Debug.Log("ArcadeInstruction " + gameObject.name + " is active: " + active);
        SetInstructionActive(active);
        SetArrowActive(active);
        SetPhoneActive(active);
    }

    private void SetPhoneActive(bool active)
    {
        if (phone != null)
        {
            phone.gameObject.SetActive(active);
            if (phone.IsLockingControls())
            {
                Server.LockPlayerControls(active);
            }
        }
    }

    private void SetInstructionActive(bool active)
    {
        instruction.SetVisible(active);
    }

    private void SetArrowActive(bool active)
    {
        if (arrow != null)
        {
            arrow.gameObject.SetActive(active);
            arrow.Reset();
        }
    }
}
