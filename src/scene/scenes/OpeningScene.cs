using UnityEngine;

/// <summary>
/// The starting scene of the game.
/// </summary>
public class OpeningScene : Scene
{
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private GameObject startPosition;
    [SerializeField]
    private Player player;

    public override void CloseScene()
    {
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        player.transform.position = startPosition.transform.position;
        musicManager.SetMusic(MusicManager.MUSIC.START);
        CloseScene();
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
