using UnityEngine;

/// <summary>
/// A transition scene between tutorial and levels.
/// </summary>
public class TutorialFinishScene : Scene
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private CollectiveInstructions instructions;
    [SerializeField]
    private BlastDoor blastDoor;
    [SerializeField]
    private MusicManager musicManager;

    private int totalFrames = 80;
    private int timer = 0;

    private void Update()
    {
        if (status == Status.Opening)
        {
            if (timer < totalFrames)
            {
                timer++;
                return;
            } else
            {
                timer = 0;
                CloseScene();
            }
        }
    }

    public override void CloseScene()
    {
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        ScoreBoard.GetInstance().StartLevel();
        musicManager.SetVoice(MusicManager.VOICE.TUTORIAL_END);
        player.ResetEntity();
        instructions.SetInstruction(CollectiveInstructions.InstType.GET_READY);
        blastDoor.Open();

        status = Status.Opening;
        timer = 0;
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
