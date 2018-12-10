using UnityEngine;

/// <summary>
/// A level with a boss. Boss can only be damaged when he is vulnerable, to prevent combo hitting.
/// </summary>
public class BossScene : LevelScene
{
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Boss boss;
    [SerializeField]
    private MusicManager musicManager;

    [SerializeField]
    private BlastDoor door;
    [SerializeField]
    private GameObject bossEscapePoint;
    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private GameObject start;

    private int timer = -1;
    private bool isBossVulnerable = false;
    private int bossAttackLoop = -1;

    private const int DAMAGE_ON_BOSS = 20;
    private const int EVENT_TIME_LEVEL_START = 0;
    private const int EVENT_TIME_TRANSITION = 60;
    private const int EVENT_TIME_START_FIGHT = 120;

    private const int FRAME_BOSS_RETREAT = 40;
    private const int FRAME_PLAYER_PURSUE = 80;
    private const int FRAME_BOSS_CHANGE = 120;
    private const int FRAME_BOSS_PRE_ATTACK = 142;
    private const int FRAME_BOSS_ATTACK = 162;
    private const float UNIT_DISTANCE = 7.0f;

    private GameObject[] bossAttackPositionMarkers = new GameObject[3];

    public override void CloseScene()
    {
        ScoreBoard.GetInstance().HideHighScores();
        timer = -1;
        isBossVulnerable = false;
        status = Status.Closed;
    }

    protected override void DetectPlayerAttack()
    {
        if (isBossVulnerable && player.CalculateWeaponPenetration(boss) > 0)
        {
            return;
        }
        isBossVulnerable = false;
        if (boss.Health <= DAMAGE_ON_BOSS)
        {
            EndLevel();
        }
        else
        {
            boss.Damage(DAMAGE_ON_BOSS);
            ScoreBoard.GetInstance().IncrementStreak();
            bossAttackLoop = FRAME_BOSS_RETREAT;
        }
    }

    protected override void HandleTimedEvents()
    {
        if (timer < EVENT_TIME_LEVEL_START)
        {
            PlayLevelStartEvents();
        }
        else if (timer == EVENT_TIME_TRANSITION)
        {
            PlayLevelTransitionEvents();
        }
        else if (timer == EVENT_TIME_START_FIGHT)
        {
            StartFight();
            return;
        }
        timer++;
    }

    protected override void AttackLoop()
    {
        if (bossAttackLoop <= -1)
        {
            return;
        }

        if (bossAttackLoop == FRAME_BOSS_RETREAT)
        {
            BeginBossRetreat();
        }
        else if (bossAttackLoop == FRAME_PLAYER_PURSUE)
        {
            BeginPlayerPursuit();
        }
        else if (bossAttackLoop == FRAME_BOSS_CHANGE)
        {
            BeginBossChangePosition();
        }
        else if (bossAttackLoop == FRAME_BOSS_PRE_ATTACK)
        {
            if (boss.GetStatus() != AEntity.Status.Idle)
            {
                return;
            }
        }
        else if (bossAttackLoop == FRAME_BOSS_ATTACK)
        {
            BeginBossAttack();
        }

        if (IsBossWaiting())
        {
            bossAttackLoop++;
        }
    }


    protected override bool AreEnemiesDead()
    {
        return boss.GetStatus() == AEntity.Status.Dead;
    }

    protected override void InitializeLevel()
    {
        Debug.Log("Initializing boss level.");
        instructions.SetInstruction(CollectiveInstructions.InstType.BOSS_STAGE);
        boss.SetRotateSpeed(12.0f);
        bossAttackPositionMarkers[LevelScene.POSITION_LEFT_INDEX] = player.GetLeft();
        bossAttackPositionMarkers[LevelScene.POSITION_FRONT_INDEX] = player.GetFront();
        bossAttackPositionMarkers[LevelScene.POSITION_RIGHT_INDEX] = player.GetRight();
    }

    protected override void MovePlayerAtSceneStart()
    {
        Debug.Log("Moving " + player.name);
        player.gameObject.transform.position = start.transform.position;
        player.Rotate(destination.transform.position);
        player.Move(destination.transform.position);
    }

    protected override void IssueCinematics()
    {
        Debug.Log("Starting cinematics.");
        door.Open();
        boss.gameObject.SetActive(true);
        boss.gameObject.transform.position = spawnPoint.transform.position;
        musicManager.SetMusic(MusicManager.MUSIC.BOSS);
        musicManager.SetVoice(MusicManager.VOICE.PRE_BOSS);
    }

    protected override void StartEvents()
    {
        Debug.Log("Starting events.");
        status = Status.Opening;
        timer = -1;
    }

    private void EndLevel()
    {
        Debug.Log("Ending boss level.");
        bossAttackLoop = -1;
        boss.Kill();
        ScoreBoard.GetInstance().ShowHighscores();
        boss.healthBar.gameObject.SetActive(false);
        player.Rotate(bossEscapePoint.transform.position);
        musicManager.SetMusic(MusicManager.MUSIC.START);
        musicManager.SetVoice(MusicManager.VOICE.GAME_END);
    }

    private void PlayLevelStartEvents()
    {
        Debug.Log("Playing level start events.");
        timer = 0;
        instructions.SetInstruction(CollectiveInstructions.InstType.BOSS_STAGE);
        door.Close();
        boss.Rotate(player.GetFront().transform.position);
    }

    private void PlayLevelTransitionEvents()
    {
        Debug.Log("Playing level transition events.");
        boss.healthBar.gameObject.SetActive(true);
        // hide level text
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        // walk the boss to the player
        boss.Rotate(player.GetFront().transform.position);
        boss.Move(player.GetFront().transform.position);
    }

    private void StartFight()
    {
        Debug.Log("Boss fight begin.");
        status = Status.Open;
        bossAttackLoop = FRAME_BOSS_CHANGE;
    }

    private void BeginBossRetreat()
    {
        Debug.Log(boss.name + " begins to retreat.");
        player.ClearActions();
        boss.ClearActions();
        boss.Rotate(player.transform.position);
        boss.Move(boss.transform.position + (boss.transform.position - player.transform.position).normalized * (2 * UNIT_DISTANCE));
        player.Rotate(boss.transform.position);
    }

    private void BeginPlayerPursuit()
    {
        Debug.Log(player.name + " pursues " + boss.name);
        // player walks towards boss
        player.Move(boss.transform.position - (boss.transform.position - player.transform.position).normalized * UNIT_DISTANCE * 1.5f);
        bossAttackLoop = FRAME_BOSS_CHANGE;
    }

    private void BeginBossChangePosition()
    {
        if (player.GetStatus() != AEntity.Status.Idle || boss.GetStatus() != AEntity.Status.Idle)
        {
            return;
        }
        Debug.Log(boss.name + " changing position.");

        int playerChoice = GetPlayerPositionChoice(player.GetControllerPosition());
        int choice = (int)Random.Range(0, 1.99f);
        if (choice == playerChoice)
        {
            choice++;
        }
        Vector3 chosenPosition = bossAttackPositionMarkers[choice].transform.position;
        boss.Rotate(chosenPosition);
        boss.Move(chosenPosition);
        bossAttackLoop = FRAME_BOSS_PRE_ATTACK;
    }

    private void BeginBossAttack()
    {
        if (boss.GetStatus() != AEntity.Status.Idle)
        {
            Debug.LogError("Boss is not idle when performing attack. Defaulting to stalling until boss is idle...");
            return;
        }
        boss.Attack(player);
        isBossVulnerable = true;
        bossAttackLoop = FRAME_BOSS_CHANGE;
    }

    private bool IsBossWaiting()
    {
        return bossAttackLoop < FRAME_PLAYER_PURSUE || bossAttackLoop >= FRAME_BOSS_PRE_ATTACK && bossAttackLoop < FRAME_BOSS_ATTACK;
    }

    private int GetPlayerPositionChoice(Player.Position playerPosition)
    {
        switch (playerPosition)
        {
            case Player.Position.left:
                return LevelScene.POSITION_LEFT_INDEX;
            case Player.Position.front:
                return LevelScene.POSITION_FRONT_INDEX;
            case Player.Position.right:
                return LevelScene.POSITION_RIGHT_INDEX;
            default:
                Debug.LogError("Player position is unexpected: " + playerPosition.ToString());
                Debug.LogError("Defaulting to front position");
                return LevelScene.POSITION_FRONT_INDEX;
        }
    }
}
