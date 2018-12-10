using UnityEngine;
/// <summary>
/// Contains information for <see cref="IGrowable"/> animation.
/// </summary>
public class LinearGrowth
{
    private float startSize;
    private float endSize;
    private int duration;
    private int currentFrame;

    public LinearGrowth(float startSize, float endSize, int duration)
    {
        this.startSize = startSize;
        this.endSize = endSize;
        this.duration = duration;
        this.currentFrame = 0;
    }

    /// <summary>
    /// Checks if the growth animation has any frames length.
    /// </summary>
    public bool HasFrame()
    {
        return (currentFrame <= duration);
    }

    /// <summary>
    /// Decrements duration by 1.
    /// </summary>
    public void Update()
    {
        duration--;
    }

    /// <summary>
    /// Gets the interpolated scale of the animation.
    /// </summary>
    /// <returns>Interpolated scale of animation.</returns>
    public float GetScale()
    {
        float result = Mathf.Lerp(startSize, endSize, currentFrame / (float) duration);
        return result;
    }

    public override string ToString()
    {
        return "startSize: " + startSize + " endSize: " + endSize + " duration: " + duration;
    }
}
