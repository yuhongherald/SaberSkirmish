using TMPro;

using UnityEngine;

/// <summary>
/// A basic class for showing a text instruction.
/// </summary>
public class Instruction : MonoBehaviour
{
    [SerializeField]
    private int animationFrames = 20;
    [SerializeField]
    private float minSize = 0;
    [SerializeField]
    private float maxSize = 1;
    [SerializeField]
    private float minAlpha = 0;
    [SerializeField]
    private float maxAlpha = 1;

    protected Vector3 scale;
    protected bool animate = false;
    protected TextMeshPro textMesh;
    protected Color color;

    private int currentFrame = 0;
    private bool visible = false;

    /// <summary>
    /// Sets the visibility of instruction.
    /// </summary>
    /// <param name="visible">Visibility to be set.</param>
    public void SetVisible(bool visible)
    {
        if (this.visible == visible)
        {
            return;
        }
        this.visible = visible;

    }

    /// <summary>
    /// Sets animation state of instruction.
    /// </summary>
    /// <param name="animate">Animation state to be set.</param>
    public void SetAnimate(bool animate)
    {
        this.animate = animate;
    }

    protected virtual void Start()
    {
        textMesh = gameObject.GetComponentInChildren<TextMeshPro>();
        color = textMesh.color;
        scale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
    }

    protected bool HasChanges()
    {
        if (visible)
        {
            if (currentFrame >= animationFrames)
            {
                return false;
            }
            else
            {
                currentFrame++;
            }
        }
        else
        {
            if (currentFrame <= 0)
            {
                return false;
            }
            else
            {
                currentFrame--;
            }
        }
        return true;
    }

    protected virtual void Update()
    {
        GrowAnim();
        Animate();
    }

    private void GrowAnim()
    {
        if (!HasChanges()) return;
        Color newColor = new Color(color.r, color.g, color.b,
            minAlpha + (maxAlpha - minAlpha) * currentFrame / animationFrames);
        textMesh.color = newColor;
        gameObject.transform.localScale = scale *
            (minSize + (maxSize - minSize) * currentFrame / animationFrames);
    }

    protected virtual void Animate()
    {
    }
}
