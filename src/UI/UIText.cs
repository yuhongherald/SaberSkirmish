using UnityEngine;
using TMPro;

/// <summary>
/// A class used to display text.
/// </summary>
public class UIText : MonoBehaviour, IGrowable
{
    [SerializeField]
    private TextMeshPro tmp;
    private Vector3 initScale;
    private LinearGrowth growth;

    /// <summary>
    /// Sets current UI text to <paramref name="text"/>.
    /// </summary>
    public void SetText(string text)
    {
        tmp.text = text;
    }

    public void Grow(float startSize, float endSize, int duration)
    {
        growth = new LinearGrowth(startSize, endSize, duration);
        Debug.Log("New LinearGrowth created: " + growth.ToString());
    }

    private void Awake()
    {
        initScale = gameObject.transform.localScale;
        growth = new LinearGrowth(1.0f, 1.0f, 0);
    }

    private void Update()
    {
        UpdateGrowth();
    }

    private void UpdateGrowth()
    {
        if (!growth.HasFrame())
        {
            return;
        }
        gameObject.transform.localScale = initScale * growth.GetScale();
        growth.Update();
    }
}
