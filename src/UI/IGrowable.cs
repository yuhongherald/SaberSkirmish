/// <summary>
/// A UI element that has a growing animation.
/// </summary>
public interface IGrowable
{
    /// <summary>
    /// Transits between <paramref name="startSize"/> and
    /// <paramref name="endSize"/> for <paramref name="duration"/>.
    /// </summary>
    void Grow(float startSize, float endSize, int duration);
}
