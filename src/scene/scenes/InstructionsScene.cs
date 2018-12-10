/// <summary>
/// A scene to demonstrate how to control the lightsaber.
/// </summary>
public class InstructionsScene : Scene
{
    public CollectiveInstructions instructions;
    public int totalFrames = 480;

    private int currentFrame = 0;

    public void Update()
    {
        if (status == Status.Open)
        {
            currentFrame++;
            if (currentFrame > totalFrames)
            {
                CloseScene();
            }
        }
    }

    public override void CloseScene()
    {
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        currentFrame = 0;
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        instructions.SetInstruction(CollectiveInstructions.InstType.DEMONSTRATE);
        currentFrame = 0;
        status = Status.Open;
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
