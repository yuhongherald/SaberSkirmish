using UnityEngine;

/// <summary>
/// A level with 3 close ranged <see cref="StormTrooper"/>.
/// </summary>
public class Level1StartScene : Scene
{
    [SerializeField]
    private CollectiveInstructions instructions;
    [SerializeField]
    private GameObject rightSpawnPoint;
    [SerializeField]
    private Player player;

    [SerializeField]
    private BlastDoor door;
    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject faceDoor;
    [SerializeField]
    private GameObject faceNext;

    [SerializeField]
    private StormTrooper trooperTemplate;

    private const int numTroopers = 3;
    private const int START = 0;
    private const int TROOPER_LEFT_START = 60;
    private const int TROOPER_RIGHT_START = 80;
    private const int TROOPER_FRONT_START = 100;
    private StormTrooper[] troopers = new StormTrooper[3];

    private int timer = -1;

    private void Update()
    {
        AttackLoop();
        if (status == Status.Opening)
        {
            if (player.GetStatus() != AEntity.Status.Idle)
            {
                return;
            }
            DetectPlayerAttack();
            PlaySceneEvents();
        }
        else if (status == Status.Open)
        {
            DetectPlayerAttack();
            WaitToCloseLevel();
        }
    }

    private void PlaySceneEvents()
    {
        if (timer < START)
        {
            door.Open();
            player.Rotate(faceDoor.transform.position);
            troopers[LevelScene.POSITION_LEFT_INDEX].Rotate(player.GetLeft().transform.position);
            troopers[LevelScene.POSITION_RIGHT_INDEX].Rotate(player.GetRight().transform.position);
            troopers[LevelScene.POSITION_FRONT_INDEX].Rotate(player.GetFront().transform.position);
            timer = START;
            instructions.SetInstruction(CollectiveInstructions.InstType.LEVEL_1);
        }
        else if (timer == TROOPER_LEFT_START)
        {
            instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
            troopers[LevelScene.POSITION_LEFT_INDEX].Move(player.GetLeft().transform.position);
        }
        else if (timer == TROOPER_RIGHT_START)
        {
            troopers[LevelScene.POSITION_RIGHT_INDEX].Move(player.GetRight().transform.position);
        }
        else if (timer == TROOPER_FRONT_START)
        {
            troopers[LevelScene.POSITION_FRONT_INDEX].Move(player.GetFront().transform.position);
        }
        else if (timer >= 120)
        {
            status = Status.Open;
        }
        timer++;
    }

    private void WaitToCloseLevel()
    {
        for (int i = 0; i < numTroopers; i++)
        {
            if (troopers[i].GetStatus() != AEntity.Status.Dead)
            {
                return;
            }
        }
        CloseScene();
    }

    private void DetectPlayerAttack()
    {
        for (int i = 0; i < numTroopers; i++)
        {
            if (player.CalculateWeaponPenetration(troopers[i]) > 0)
            {
                troopers[i].Kill();
            }
        }
    }

    private void AttackLoop()
    {
        if (timer < START)
        {
            return;
        }
        if (timer > TROOPER_LEFT_START && troopers[LevelScene.POSITION_LEFT_INDEX].GetStatus() == AEntity.Status.Idle)
        {
            troopers[LevelScene.POSITION_LEFT_INDEX].Attack(player);
        }
        if (timer > TROOPER_RIGHT_START && troopers[LevelScene.POSITION_RIGHT_INDEX].GetStatus() == AEntity.Status.Idle)
        {
            troopers[LevelScene.POSITION_RIGHT_INDEX].Attack(player);
        }
        if (timer > TROOPER_FRONT_START && troopers[LevelScene.POSITION_FRONT_INDEX].GetStatus() == AEntity.Status.Idle)
        {
            troopers[LevelScene.POSITION_FRONT_INDEX].Attack(player);
        }

    }

    public override void CloseScene()
    {
        player.Rotate(faceNext.transform.position);
        door.Close();
        for (int i = 0; i < numTroopers; i++)
        {
            troopers[i].gameObject.SetActive(false);
        }
        timer = -1;
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        if (status == Status.Uninitialized)
        {
            InitializeStormTroopers();
        }
        ResetStormTroopers();
        ResetPlayerPosition();

        player.Rotate(destination.transform.position);
        player.Move(destination.transform.position);

        status = Status.Opening;
        timer = -1;
    }

    private void ResetPlayerPosition()
    {
        player.gameObject.transform.position = start.transform.position;
    }

    private void ResetStormTroopers()
    {
        for (int i = 0; i < numTroopers; i++)
        {
            troopers[i].gameObject.SetActive(true);
            troopers[i].gameObject.transform.position = rightSpawnPoint.transform.position;
        }
    }

    private void InitializeStormTroopers()
    {
        for (int i = 0; i < numTroopers; i++)
        {
            troopers[i] = GameObject.Instantiate(trooperTemplate);
        }
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
