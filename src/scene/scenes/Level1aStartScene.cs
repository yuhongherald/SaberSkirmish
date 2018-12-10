using UnityEngine;

/// <summary>
/// A level with 3 far ranged <see cref="StormTrooper"/>.
/// </summary>
public class Level1aStartScene : LevelScene
{
    [SerializeField]
    private GameObject frontSpawnPoint;
    [SerializeField]
    private GameObject turnPoint;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private StormTrooper trooperTemplate;

    private const int NUM_TROOPERS = 3;
    private const int EVENT_TIME_LEFT_TROOPER = 0;
    private const int EVENT_TIME_FRONT_TROOPER = 80;
    private const int EVENT_TIME_RIGHT_TROOPER = 140;
    private const int EVENT_TIME_END = 180;
    private const float TROOPER_MOVE_SPEED = 0.4f;
    private const float TROOPER_ROTATE_SPEED = 12f;

    private StormTrooper[] troopers = new StormTrooper[NUM_TROOPERS];

    private int timer = -1;

    public override void CloseScene()
    {
        Debug.Log("Closing scene...");
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopers[i].gameObject.SetActive(false);
        }
        timer = -1;
        status = Status.Closed;
    }

    protected override void DetectPlayerAttack()
    {
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopers[i].AttemptToDeflectLaser(player);
        }
    }

    protected override void HandleTimedEvents()
    {
        if (timer < EVENT_TIME_LEFT_TROOPER)
        {
            SpawnLeftTrooper();
        }
        else if (timer == EVENT_TIME_FRONT_TROOPER)
        {
            SpawnFrontTrooper();
            // hide level text
            instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        }
        else if (timer == EVENT_TIME_RIGHT_TROOPER)
        {
            SpawnRightTrooper();
        }
        else if (timer >= EVENT_TIME_END)
        {
            status = Status.Open;
            return;
        }
        timer++;
    }

    protected override void AttackLoop()
    {
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            if (troopers[i].GetStatus() == AEntity.Status.Idle)
            {
                Debug.Log(troopers[i].name + " starts attacking " + player.name);
                troopers[i].Attack(player);
            }
        }
    }

    protected override bool AreEnemiesDead()
    {
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            if (troopers[i].GetStatus() != AEntity.Status.Dead)
            {
                return false;
            }
        }
        return true;
    }

    protected override void InitializeLevel()
    {
        Debug.Log("Initializing Level 1a");
        instructions.SetInstruction(CollectiveInstructions.InstType.LEVEL_1);
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopers[i] = Instantiate(trooperTemplate);
            troopers[i].SetMoveSpeed(TROOPER_MOVE_SPEED);
            troopers[i].SetRotateSpeed(TROOPER_ROTATE_SPEED);
        }
    }

    protected override void MovePlayerAtSceneStart()
    {
        Debug.Log("Moving " + player.name);
        player.gameObject.transform.position = start.transform.position;
        player.Move(start.transform.position);
    }

    protected override void IssueCinematics()
    {
        Debug.Log("Starting cinemetics.");
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopers[i].gameObject.SetActive(true);
            troopers[i].gameObject.transform.position = frontSpawnPoint.transform.position;
        }
    }

    protected override void StartEvents()
    {
        Debug.Log("Starting events");
        status = Status.Opening;
        timer = -1;
    }

    private void SpawnLeftTrooper()
    {
        timer = 0;
        Debug.Log("Spawning Left Stormtrooper.");
        StormTrooper trooper = troopers[LevelScene.POSITION_LEFT_INDEX];
        trooper.Rotate(turnPoint.transform.position);
        trooper.Move(turnPoint.transform.position);
        trooper.Rotate(player.GetFarLeft());
        trooper.Move(player.GetFarLeft());
    }

    private void SpawnFrontTrooper()
    {
        Debug.Log("Spawning Front Stormtrooper.");
        StormTrooper trooper = troopers[LevelScene.POSITION_FRONT_INDEX];
        trooper.Rotate(turnPoint.transform.position);
        trooper.Move(turnPoint.transform.position);
        trooper.Rotate(player.GetFarFront());
        trooper.Move(player.GetFarFront());
    }

    private void SpawnRightTrooper()
    {
        Debug.Log("Spawning Right Stormtrooper.");
        StormTrooper trooper = troopers[LevelScene.POSITION_RIGHT_INDEX];
        trooper.Rotate(turnPoint.transform.position);
        trooper.Move(turnPoint.transform.position);
        trooper.Rotate(player.GetFarRight());
        trooper.Move(player.GetFarRight());
    }
}
