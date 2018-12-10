using UnityEngine;

/// <summary>
/// A named enemy entity with a <see cref="HealthBar"/>.
/// </summary>
public class Boss : HealthBarEntity
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Door escapeDoor;
    [SerializeField]
    private GameObject escapePoint;

    private const int ATTACK_POINT1 = 12;
    private const int ATTACK_POINT2 = 20;
    private const int TOTAL_ATTACK_FRAMES = 50;
    private const int FRAMES_AFTER_DEATH = 1080;
    private const int DAMAGE_ON_ATTACKED_ENTITY = 10;

    private int currentAttackFrame = -1;
    private int currentDeathFrame = -1;

    public int Health
    {
        get
        {
            return currentHealth;
        }
    }

    public override float MoveSpeed
    {
        get
        {
            return 1.0f;
        }
    }

    protected override void ResetEntityMesh()
    {
        escapeDoor.Close();
        base.ResetEntityMesh();
    }

    protected override bool KillAction(bool placeholder)
    {
        if (currentDeathFrame == -1)
        {
            escapeDoor.Open();
            currentDeathFrame = 0;
            return false;
        }
        else if (currentDeathFrame < FRAMES_AFTER_DEATH)
        {
            FollowAction(escapePoint);
            currentDeathFrame++;
            return false;
        }
        else
        {
            currentDeathFrame = -1;
            gameObject.SetActive(false); // hope this works
            return true;
        }

    }

    protected override bool AttackAction(AEntity entity)
    {
        if (!RotateAction(entity.gameObject.transform.position))
        {
            return false;
        }
        if (currentAttackFrame == -1)
        {
            Debug.Log("Boss is attacking");
            animator.SetTrigger("Attack1Trigger");
            currentAttackFrame = 0;
            audioSource.Play();
            return false;
        }
        else if (currentAttackFrame < TOTAL_ATTACK_FRAMES)
        {
            if (currentAttackFrame == ATTACK_POINT1 || currentAttackFrame == ATTACK_POINT2)
            {
                entity.Damage(DAMAGE_ON_ATTACKED_ENTITY);
            }
            currentAttackFrame++;
            return false;
        }
        else
        {
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
            animator.SetBool("Moving", false);
            gameObject.transform.position = destination;
            return true;
        }
        else
        {
            animator.SetBool("Moving", true);
            gameObject.transform.position = gameObject.transform.position
                + movement.normalized * MoveSpeed;
            return false;
        }
    }

    protected override bool RotateAction(Vector3 destination)
    {
        Vector3 lookDirection = destination - gameObject.transform.position;
        Quaternion lookRotation;
        if (lookDirection.magnitude > 0.001)
        {
            lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        } else
        {
            lookRotation = transform.rotation;
            return true;
        }
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
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,
                globalRotation, rotateSpeed / angle);
            return false;
        }
    }
}
