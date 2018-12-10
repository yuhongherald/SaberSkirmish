using System;

using UnityEngine;

/// <summary>
/// A class used to sequence events in the game.
/// </summary>
[Serializable]
public abstract class Scene : MonoBehaviour
{
    public enum Status
    {
        Uninitialized,
        Opening,
        Open,
        Closing,
        Closed,
        Length
    }
    protected Status status = Status.Uninitialized;

    /// <summary>
    /// Opens the scene. Called by <see cref="SceneEngine"/>.
    /// </summary>
    public abstract void OpenScene();

    /// <summary>
    /// Closes the scene so that <see cref="SceneEngine"/> can open the next scene.
    /// </summary>
    public abstract void CloseScene();

    /// <summary>
    /// Releases resources associated with scene.
    /// </summary>
    public abstract void Teardown();

    /// <summary>
    /// Gets the current status of the scene.
    /// </summary>
    public virtual Status GetStatus()
    {
        return status;
    }
}
