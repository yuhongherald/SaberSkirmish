using UnityEngine;

using TMPro;

/// <summary>
/// Instructional text that creates ripples converging around text.
/// </summary>
public class RippleInstruction : Instruction
{
    [SerializeField]
    private float minRippleSize = 1.6f;
    [SerializeField]
    private float maxRippleSize = 1.1f;
    [SerializeField]
    private float minRippleAlpha = 0.1f;
    [SerializeField]
    private float maxRippleAlpha = 0.8f;

    [SerializeField]
    private int numberRipples = 3;
    [SerializeField]
    private int rippleFrame = 6;
    [SerializeField]
    private int rippleLifespan = 18;

    [SerializeField]
    private int rippleLoop = 40;

    private TextMeshPro[] ripples;
    private int currentFrame = 0;

    protected override void Start()
    {
        base.Start();
        InitRipples();
    }

    private void InitRipples()
    {
        ripples = new TextMeshPro[numberRipples];
        for (int i = 0; i < numberRipples; i++)
        {
            ripples[i] = TextMeshPro.Instantiate(textMesh);
        }
        for (int i = 0; i < numberRipples; i++)
        {
            ripples[i].transform.parent = textMesh.transform;
        }
    }

    protected override void Animate()
    {
        currentFrame++;
        SetRipplesActive(true);
        AnimateRipples();
    }

    private void SetRipplesActive(bool active)
    {
        for (int i = 0; i < numberRipples; i++)
        {
            ripples[i].gameObject.SetActive(active);
        }
    }

    private void AnimateRipples()
    {
        for (int i = 0; i < numberRipples; i++)
        {
            AnimateRipple(i);
        }
    }

    private void AnimateRipple(int i)
    {
        int localFrame = (currentFrame - i * rippleFrame + rippleLoop) % rippleLoop;
        if (localFrame > rippleLifespan)
        {
            ripples[i].gameObject.SetActive(false);
        }
        else
        {
            Color newColor = new Color(color.r, color.g, color.b, minRippleAlpha + (maxRippleAlpha - minRippleAlpha) * localFrame / rippleLifespan);
            ripples[i].color = newColor;
            ripples[i].gameObject.transform.localScale = Vector3.one * (minRippleSize + (maxRippleSize - minRippleSize) * localFrame / rippleLifespan);
        }
    }
}
