using UnityEngine;

/// <summary>
/// An animated spaceship blast door.
/// </summary>
public class BlastDoor : Door
{
    [SerializeField]
    private GameObject leftPanel;
    [SerializeField]
    private GameObject rightPanel;
    [SerializeField]
    private Vector3 leftPanelClosed;
    [SerializeField]
    private Vector3 rightPanelClosed;
    [SerializeField]
    private Vector3 leftPanelOpen = new Vector3(0, 0, -140);
    [SerializeField]
    private Vector3 rightPanelOpen = new Vector3(0, 0, -140);

    private int currentFrame = 0;
    private int totalFrames = 40;

    private bool closing = true;

    private void Update()
    {
        if (closing)
        {
            if (currentFrame > 0)
            {
                currentFrame--;
                leftPanel.transform.localPosition = 
                    Vector3.Lerp(leftPanelClosed, leftPanelOpen, (float)currentFrame / (float)totalFrames);
                rightPanel.transform.localPosition = 
                    Vector3.Lerp(rightPanelClosed, rightPanelOpen, (float)currentFrame / (float)totalFrames);
            }
        } else
        {
            if (currentFrame < totalFrames)
            {
                currentFrame++;
                leftPanel.transform.localPosition = 
                    Vector3.Lerp(leftPanelClosed, leftPanelOpen, (float)currentFrame / (float)totalFrames);
                rightPanel.transform.localPosition = 
                    Vector3.Lerp(rightPanelClosed, rightPanelOpen, (float)currentFrame / (float)totalFrames);
            }
        }
    }

    public override void Close()
    {
        closing = true;
    }

    public override void Open()
    {
        closing = false;
    }
}
