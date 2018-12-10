using UnityEngine;

/// <summary>
/// A class that manages the main logic loop of the game, with events sequence in <see cref="Scene"/>.
/// </summary>
public class SceneEngine : MonoBehaviour
{
    public Scene[] scenes;
    public Server server;
    public int startScene = 0;
    private int sceneNumber;

    private void Start()
    {
        if (scenes.Length == 0)
        {
            return;
        }
        sceneNumber = startScene;
        Debug.Log("Opening scene: " + sceneNumber);
        scenes[sceneNumber].OpenScene();
    }

    private void Update()
    {
        if (scenes[sceneNumber].GetStatus() != Scene.Status.Closed || server.IsShowingQr())
        {
            return;
        }
        sceneNumber = (sceneNumber + 1);
        if (sceneNumber >= scenes.Length)
        {
            sceneNumber = 0;
            server.DropPlayer();
        }
        Debug.Log("Opening scene: " + sceneNumber);
        scenes[sceneNumber].OpenScene();
    }
}
