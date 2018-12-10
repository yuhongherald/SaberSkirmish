using UnityEngine;

/// <summary>
/// An abstract base scene class with more advanced features such as event management.
/// </summary>
public abstract class LevelScene : Scene
{
    [SerializeField]
    protected CollectiveInstructions instructions;
    [SerializeField]
    protected Player player;

    public const int POSITION_LEFT_INDEX = 0;
    public const int POSITION_RIGHT_INDEX = 1;
    public const int POSITION_FRONT_INDEX = 2;

    public override void OpenScene()
    {
        Debug.Log("Opening Scene");
        if (status == Status.Uninitialized)
        {
            InitializeLevel();
        }
        MovePlayerAtSceneStart();
        IssueCinematics();
        StartEvents();
    }

    public override void Teardown()
    {
        Debug.LogError("Attempted to tear down level scene. Currently unimplemented.");
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called every frame by Unity Engine.
    /// </summary>
    private void Update()
    {
        if (status == Status.Opening)
        {
            if (player.GetStatus() != AEntity.Status.Idle)
            {
                return;
            }
            DetectPlayerAttack();
            HandleTimedEvents();
        }
        else if (status == Status.Open)
        {
            AttackLoop();
            DetectPlayerAttack();
            if (AreEnemiesDead())
            {
                CloseScene();
            }
        }
    }

    /// <summary>
    /// Handles player attack events
    /// </summary>
    protected abstract void DetectPlayerAttack();

    /// <summary>
    /// Handles timed events such as enemy spawns.
    /// </summary>
    protected abstract void HandleTimedEvents();

    /// <summary>
    /// Handles attack actions by all enemies on this level.
    /// </summary>
    protected abstract void AttackLoop();

    /// <summary>
    /// Checks if all enemies in the scene is dead.
    /// </summary>
    /// <returns></returns>
    protected abstract bool AreEnemiesDead();

    /// <summary>
    /// Used if level is not initialized.
    /// </summary>
    protected abstract void InitializeLevel();

    /// <summary>
    /// Moves the player at the start of the level.
    /// </summary>
    protected abstract void MovePlayerAtSceneStart();

    /// <summary>
    /// Starts cinematics for the level
    /// </summary>
    protected abstract void IssueCinematics();

    /// <summary>
    /// Starts events of the level.
    /// </summary>
    protected abstract void StartEvents();
}
