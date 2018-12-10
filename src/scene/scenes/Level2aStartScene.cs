using UnityEngine;

public class Level2aStartScene : LevelScene
{
    [SerializeField]
    private GameObject destination1;
    [SerializeField]
    private GameObject destination2;
    [SerializeField]
    private GameObject start;

    [SerializeField]
    private GameObject leftSpawnPoint;
    [SerializeField]
    private BlastDoor leftDoor;
    [SerializeField]
    private GameObject rightSpawnPoint;
    [SerializeField]
    private BlastDoor rightDoor;
    [SerializeField]
    private GameObject frontSpawnPoint1;
    [SerializeField]
    private GameObject frontSpawnPoint2;
    [SerializeField]
    private GameObject frontSpawnTurnPoint;
    [SerializeField]
    private BlastDoor frontDoor;
    [SerializeField]
    private StormTrooper trooperTemplate;

    private const int NUM_TROOPERS = 4;
    private const int EVENT_TIME_FRONT_TROOPER = 0;
    private const int EVENT_TIME_LEFT_TROOPER = 120;
    private const int EVENT_TIME_RIGHT_TROOPER = 160;
    private const int EVENT_TIME_OPEN_SIDE_DOORS = 80;
    private const float TROOPER_MOVE_SPEED = 0.4f;
    private const float TROOPER_FRONT_MOVE_SPEED = 1.2f;
    private const float TROOPER_ROTATE_SPEED = 12f;

    private StormTrooper[] troopersLeft = new StormTrooper[NUM_TROOPERS];
    private StormTrooper[] troopersRight = new StormTrooper[NUM_TROOPERS];
    private StormTrooper[] troopersFront = new StormTrooper[NUM_TROOPERS];
    private int trooperLeftIndex;
    private int trooperRightIndex;
    private int trooperFrontIndex;
    private StormTrooper[] far = new StormTrooper[3];
    private StormTrooper[] near = new StormTrooper[3];

    private int timer = -1;

    public override void CloseScene()
    {
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopersLeft[i].gameObject.SetActive(false);
            troopersRight[i].gameObject.SetActive(false);
            troopersFront[i].gameObject.SetActive(false);
        }
        timer = -1;
        status = Status.Closed;
    }

    protected override void DetectPlayerAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            if (far[i] != null)
            {
                far[i].AttemptToDeflectLaser(player);
            }
            if (near[i] != null && player.CalculateWeaponPenetration(near[i]) > 0)
            {
                near[i].Kill();
            }
        }
    }

    protected override void HandleTimedEvents()
    {
        RemoveDeadTrooperFromArray();
        if (timer <= EVENT_TIME_FRONT_TROOPER && trooperFrontIndex < NUM_TROOPERS)
        {
            SpawnFrontTrooper();
        }
        else if (timer == EVENT_TIME_OPEN_SIDE_DOORS)
        {
            leftDoor.Open();
            rightDoor.Open();
            // hide level text
            instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        }
        else if (timer == EVENT_TIME_LEFT_TROOPER && trooperLeftIndex < NUM_TROOPERS)
        {
            SpawnLeftTrooper();
        }
        else if (timer == EVENT_TIME_RIGHT_TROOPER && trooperRightIndex < NUM_TROOPERS)
        {
            SpawnRightTrooper();
        }
        else if (HasFinishedSpawning())
        {
            status = Status.Open;
        }
        timer++;
        timer %= 200;
    }

    protected override void AttackLoop()
    {
        for (int i = 0; i < 3; i++)
        {
            //continue if not correct time window
            if (!(2 * i * 30 <= timer && timer < (2 * i + 1) * 30))
            {
                continue;
            }
            if (far[i] != null && (near[i] == null || near[i].GetStatus() == AEntity.Status.Moving) && far[i].GetStatus() == AEntity.Status.Idle)
            {
                Debug.Log(far[i].name + " starts attacking " + player.name);
                far[i].Attack(player);
            }
            if (near[i] != null && near[i].GetStatus() == AEntity.Status.Idle)
            {
                Debug.Log(near[i].name + " starts attacking " + player.name);
                near[i].Attack(player);
            }
        }
    }

    protected override bool AreEnemiesDead()
    {
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            if (troopersLeft[i].GetStatus() != AEntity.Status.Dead)
            {
                return false;
            }
            if (troopersRight[i].GetStatus() != AEntity.Status.Dead)
            {
                return false;
            }
            if (troopersFront[i].GetStatus() != AEntity.Status.Dead)
            {
                return false;
            }
        }
        return true;
    }

    protected override void InitializeLevel()
    {
        Debug.Log("Initializing Level 2a.");
        instructions.SetInstruction(CollectiveInstructions.InstType.LEVEL_1);
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            troopersLeft[i] = Instantiate(trooperTemplate);
            troopersLeft[i].SetMoveSpeed(TROOPER_MOVE_SPEED);
            troopersLeft[i].SetRotateSpeed(TROOPER_ROTATE_SPEED);

            troopersRight[i] = Instantiate(trooperTemplate);
            troopersRight[i].SetMoveSpeed(TROOPER_MOVE_SPEED);
            troopersRight[i].SetRotateSpeed(TROOPER_ROTATE_SPEED);

            troopersFront[i] = Instantiate(trooperTemplate);
            troopersFront[i].SetMoveSpeed(TROOPER_FRONT_MOVE_SPEED);
            troopersFront[i].SetRotateSpeed(TROOPER_ROTATE_SPEED);
        }
    }

    protected override void MovePlayerAtSceneStart()
    {
        Debug.Log("Moving " + player.name);
        player.gameObject.transform.position = start.transform.position;
        player.Rotate(destination1.transform.position);
        player.Move(destination1.transform.position);
        player.Rotate(destination2.gameObject.transform.position);
        player.Move(destination2.transform.position);
    }

    protected override void IssueCinematics()
    {
        Debug.Log("Starting cinematics");
        trooperLeftIndex = 0;
        trooperRightIndex = 0;
        trooperFrontIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            far[i] = null;
            near[i] = null;
        }
        frontDoor.Open();
        for (int i = 0; i < NUM_TROOPERS; i++)
        {
            PlaceStormTroopers(i);
        }
    }

    protected override void StartEvents()
    {
        Debug.Log("Starting events.");
        status = Status.Opening;
        timer = -1;
    }

    private void RemoveDeadTrooperFromArray()
    {
        for (int i = 0; i < 3; i++)
        {
            if (far[i] != null && (far[i].GetStatus() == AEntity.Status.Dying || far[i].GetStatus() == AEntity.Status.Dead))
            {
                Debug.Log(far[i].name + " was removed from level array");
                far[i] = null;
            }
            if (near[i] != null && (near[i].GetStatus() == AEntity.Status.Dying || near[i].GetStatus() == AEntity.Status.Dead))
            {
                Debug.Log(near[i].name + " was removed from level array");
                near[i] = null;
            }
        }
    }

    private void SpawnFrontTrooper()
    {
        Debug.Log("Attempting to spawn front trooper.");
        if (far[LevelScene.POSITION_FRONT_INDEX] == null)
        {
            Debug.Log("Spawning front trooper at far range.");
            StormTrooper trooper = troopersFront[trooperFrontIndex];
            far[LevelScene.POSITION_FRONT_INDEX] = trooper;
            trooperFrontIndex++;
            trooper.Rotate(frontSpawnTurnPoint.transform.position);
            trooper.Move(frontSpawnTurnPoint.transform.position);
            trooper.Rotate(player.GetFarFront());
            trooper.Move(player.GetFarFront());
        }
        else if (near[LevelScene.POSITION_FRONT_INDEX] == null)
        {
            Debug.Log("Spawning front trooper at near range.");
            StormTrooper trooper = troopersFront[trooperFrontIndex];
            near[LevelScene.POSITION_FRONT_INDEX] = trooper;
            trooperFrontIndex++;
            trooper.Rotate(frontSpawnTurnPoint.transform.position);
            trooper.Move(frontSpawnTurnPoint.transform.position);
            trooper.Rotate(player.GetFront().transform.position);
            trooper.Move(player.GetFront().transform.position);
        }
    }

    private void SpawnLeftTrooper()
    {
        Debug.Log("Attempting to spawn left trooper.");
        if (far[LevelScene.POSITION_LEFT_INDEX] == null)
        {
            Debug.Log("Spawning left trooper at far range.");
            StormTrooper trooper = troopersLeft[trooperLeftIndex];
            far[LevelScene.POSITION_LEFT_INDEX] = trooper;
            trooperLeftIndex++;
            trooper.Rotate(player.GetFarLeft());
            trooper.Move(player.GetFarLeft());
        }
        else if (near[LevelScene.POSITION_LEFT_INDEX] == null)
        {
            Debug.Log("Spawning left trooper at near range.");
            StormTrooper trooper = troopersLeft[trooperLeftIndex];
            near[LevelScene.POSITION_LEFT_INDEX] = trooper;
            trooperLeftIndex++;
            trooper.Rotate(player.GetLeft().transform.position);
            trooper.Move(player.GetLeft().transform.position);
        }
    }

    private void SpawnRightTrooper()
    {
        Debug.Log("Attempting to spawn right trooper.");
        if (far[LevelScene.POSITION_RIGHT_INDEX] == null)
        {
            Debug.Log("Spawning right trooper at far range.");
            StormTrooper trooper = troopersRight[trooperRightIndex];
            far[LevelScene.POSITION_RIGHT_INDEX] = trooper;
            trooperRightIndex++;
            trooper.Rotate(player.GetFarRight());
            trooper.Move(player.GetFarRight());
        }
        else if (near[LevelScene.POSITION_RIGHT_INDEX] == null)
        {
            Debug.Log("Spawning right trooper at near range.");
            StormTrooper trooper = troopersRight[trooperRightIndex];
            near[LevelScene.POSITION_RIGHT_INDEX] = trooper;
            trooperRightIndex++;
            trooper.Rotate(player.GetRight().transform.position);
            trooper.Move(player.GetRight().transform.position);
        }
    }

    private bool HasFinishedSpawning()
    {
        return trooperFrontIndex >= NUM_TROOPERS &&
            trooperLeftIndex >= NUM_TROOPERS &&
            trooperRightIndex >= NUM_TROOPERS;
    }

    private void PlaceStormTroopers(int index)
    {
        troopersLeft[index].gameObject.SetActive(true);
        troopersLeft[index].gameObject.transform.position = leftSpawnPoint.transform.position;
        troopersRight[index].gameObject.SetActive(true);
        troopersRight[index].gameObject.transform.position = rightSpawnPoint.transform.position;
        troopersFront[index].gameObject.SetActive(true);
        if (index % 2 == 0)
        {
            troopersFront[index].gameObject.transform.position = frontSpawnPoint1.transform.position;
        }
        else
        {
            troopersFront[index].gameObject.transform.position = frontSpawnPoint2.transform.position;
        }
    }
}
