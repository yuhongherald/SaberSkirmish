using UnityEngine;

/// <summary>
/// The abstracted behavior of a door in the game.
/// </summary>
public abstract class Door : MonoBehaviour
{
    /// <summary>
    /// Starts opening the door.
    /// </summary>
    public abstract void Open();

    /// <summary>
    /// Starts closing the door.
    /// </summary>
    public abstract void Close();
}
