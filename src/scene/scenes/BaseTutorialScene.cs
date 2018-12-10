using UnityEngine;

/// <summary>
/// A tutorial scene that provides a practice <see cref="StormTrooper"/> and
/// instructions to attack.
/// </summary>
public class BaseTutorialScene : Scene
{
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private CollectiveInstructions instructions;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Player player;
    [SerializeField]
    private StormTrooper hologram;

    protected virtual Player.Position PLAYER_POSITION
    {
        get
        {
            return Player.Position.left;
        }
    }

    protected virtual CollectiveInstructions.InstType INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.SWING_LEFT;
        }
    }

    protected virtual CollectiveInstructions.InstType ATTACK_INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.ATTACK;
        }
    }

    private Vector3 Destination
    {
        get
        {
            return player.GetPosition(PLAYER_POSITION);
        }
    }

    private void Update()
    {
        if (status == Status.Opening)
        {
            WaitForHologramIdle();
        }
        else if (status == Status.Open)
        {
            CheckPlayerAttack();
            ShowInstruction();
            UpdateLevelStatus();
        }
    }

    private void WaitForHologramIdle()
    {
        if (hologram.GetStatus() == AEntity.Status.Idle)
        {
            status = Status.Open;
        }
    }

    private void CheckPlayerAttack()
    {
        if (PLAYER_POSITION == Player.Position.farFront)
        {
            hologram.AttemptToDeflectLaser(player);
        }
        else
        {
            if (player.CalculateWeaponPenetration(hologram) > 0)
            {
                hologram.Kill();
            }
        }
    }

    private void UpdateLevelStatus()
    {
        if (hologram.GetStatus() == AEntity.Status.Idle)
        {
            hologram.Attack(player);
        }
        else if (hologram.GetStatus() == AEntity.Status.Dying)
        {
            instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        }
        else if (hologram.GetStatus() == AEntity.Status.Dead)
        {
            CloseScene();
        }
    }

    private void ShowInstruction()
    {
        if (player.GetControllerPosition() == PLAYER_POSITION ||
            (player.GetControllerPosition() == Player.Position.front &&
            PLAYER_POSITION == Player.Position.farFront))
        {
            instructions.SetInstruction(ATTACK_INST_TYPE);
        }
        else
        {
            instructions.SetInstruction(INST_TYPE);
        }
    }

    public override void CloseScene()
    {
        if (hologram)
        {
            hologram.Kill();
        }
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        PlayVoiceInstruction();
        ResetHologram();

        hologram.Rotate(Destination);
        hologram.Move(Destination);
        status = Status.Opening;
    }

    private void PlayVoiceInstruction()
    {
        if (PLAYER_POSITION == Player.Position.left)
        {
            musicManager.SetVoice(MusicManager.VOICE.TUTORIAL_START);
        }
        else if (PLAYER_POSITION == Player.Position.farFront)
        {
            musicManager.SetVoice(MusicManager.VOICE.TUTORIAL_RANGED);
        }
    }

    private void ResetHologram()
    {
        hologram.gameObject.SetActive(true);
        hologram.gameObject.transform.position = spawnPoint.transform.position;
        hologram.ResetEntity();
        Vector3 lookDirection = player.transform.position - hologram.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        hologram.transform.rotation = lookRotation;
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
