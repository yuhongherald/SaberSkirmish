using UnityEngine;

/// <summary>
/// A generic entity class for enemies that attacks using <see cref="Laser"/>.
/// </summary>
public class StormTrooper : AEntity
{
    [SerializeField]
    private Actions actions;
    [SerializeField]
    private GameObject laserTemplate;
    [SerializeField]
    private GameObject muzzle;

    private GameObject laserObject;
    private Laser laser;
    private int totalAttackFrames = 120;
    private int currentAttackFrame = -1;
    private int totalDeathFrames = 120;
    private int currentDeathFrame = -1;

    private float pMoveSpeed = 0.15f;

    public override float MoveSpeed
    {
        get
        {
            return pMoveSpeed;
        }
    }

    /// <summary>
    /// Sets the movespeed of the StormTrooper.
    /// </summary>
    public void SetMoveSpeed(float newMoveSpeed)
    {
        pMoveSpeed = newMoveSpeed;
    }

    /// <summary>
    /// Tries to deflect the laser using the player's weapon.
    /// </summary>
    /// <param name="player"></param>
    public void AttemptToDeflectLaser(Player player)
    {
        laser.AttemptToDeflectLaser(player);
    }

    protected override bool KillAction(bool placeholder)
    {
        if (currentDeathFrame == -1)
        {
            actions.Death();
            currentDeathFrame = 0;
            return false;
        } else if (currentDeathFrame < totalDeathFrames)
        {
            currentDeathFrame++;
            return false;
        } else
        {
            currentDeathFrame = -1;
            gameObject.SetActive(false);
            return true;
        }
    }

    protected override bool AttackAction(AEntity entity)
    {
        if (laserObject == null)
        {
            InitLaser();
        }
        if (!RotateAction(entity.gameObject.transform.position)) return false; // not done rotating
        if (currentAttackFrame == -1)
        {
            Debug.Log("StormTrooper attacking");
            actions.Attack();
            currentAttackFrame = 0;
            return false;
        } else if (currentAttackFrame < totalAttackFrames)
        {
            currentAttackFrame++;
            return false;
        } else
        {
            audioSource.volume = 0.5f;
            audioSource.Play();
            setupLaser(entity);
            currentAttackFrame = -1;
            return true;
        }
    }

    protected override bool FollowAction(GameObject target)
    {
        RotateAction(target.transform.position);
        MoveAction(target.transform.position);
        return false;
    }

    protected override bool MoveAction(Vector3 destination)
    {
        Vector3 movement = (destination - gameObject.transform.position);
        if (movement.magnitude < MoveSpeed)
        {
            gameObject.transform.position = destination;
            actions.Stay();
            return true;
        }
        else
        {
            gameObject.transform.position = gameObject.transform.position + movement.normalized * MoveSpeed;
            actions.Run();
            return false;
        }
    }

    protected override bool RotateAction(Vector3 destination)
    {
        Vector3 lookDirection = destination - gameObject.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        return RotateAction(lookRotation);
    }

    protected override bool RotateAction(Quaternion globalRotation)
    {
        float angle = Quaternion.Angle(gameObject.transform.rotation, globalRotation);
        if (angle < rotateSpeed)
        {
            gameObject.transform.rotation = globalRotation;
            return true;
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, globalRotation, rotateSpeed / angle);
            return false;
        }
    }

    private void InitLaser()
    {
        laserObject = GameObject.Instantiate(laserTemplate);
        laser = laserObject.AddComponent<Laser>();
        laser.SetSource(this);
        laserObject.SetActive(false);
    }

    private void setupLaser(AEntity entity)
    {
        laserObject.SetActive(true);
        Vector3 correctedPos = entity.transform.position + new Vector3(0, 2.8f, 0);
        Quaternion quaternion = Quaternion.LookRotation(correctedPos - muzzle.transform.position, Vector3.up);
        laserObject.transform.position = muzzle.transform.position;
        laserObject.transform.rotation = quaternion;

        Vector3 laserDirection = (correctedPos - muzzle.transform.position).normalized;
        laser.AimLaser(laserDirection, correctedPos, entity);
    }
}
