using UnityEngine;

/// <summary>
/// A UI class used to display health of an <see cref="AEntity"/>.
/// </summary>
public abstract class HealthBar : MonoBehaviour
{
    protected int totalHealth = 100;
    protected int currentHealth = 100;
    protected int currentMark = 100;
    protected float depletionRate = 0.02f;
    /// <summary>
    /// Sets the health displayed.
    /// </summary>
    /// <param name="health">Health to be displayed.</param>
    public abstract void SetHealth(int health);

    /// <summary>
    /// Sets the visibility of the health bar.
    /// </summary>
    /// <param name="show">Whether the health bar is showing.</param>
    public abstract void Show(bool show);
}
