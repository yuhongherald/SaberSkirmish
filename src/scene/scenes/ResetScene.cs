using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A scene that resets the game to prepare for <see cref="OpeningScene"/>.
/// </summary>
public class ResetScene : Scene
{
    [SerializeField]
    private Image blueScreen;
    private Player player;
    private HealthBar healthBar;
    private CollectiveInstructions instructions;
    private AEntity[] entities;
    private Door[] doors;
    private GameObject startPosition;

    public override void CloseScene()
    {
        ResetUIElements();
        ResetPlayer();
        ResetEntities();
        CloseDoors();
        status = Status.Closed;
    }

    private void ResetUIElements()
    {
        Color color = blueScreen.color;
        color.a = 1.0f;
        blueScreen.color = color;
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
    }

    private void CloseDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Close();
        }
    }

    private void ResetEntities()
    {
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i].ResetEntity();
        }
    }

    private void ResetPlayer()
    {
        player.ResetEntity();
        player.transform.rotation = Quaternion.identity;
        player.transform.position = startPosition.transform.position;
        healthBar.SetHealth(100);
        healthBar.Show(false);
    }

    public override void OpenScene()
    {
        CloseScene();
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
