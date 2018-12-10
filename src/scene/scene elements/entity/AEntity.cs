using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Basic class for game entities.
/// </summary>
public abstract class AEntity : MonoBehaviour
{
    public const float MOVE_SPEED = 0.15f;
    public const float ROTATE_SPEED = 3.0f;

    public virtual float MoveSpeed
    {
        get
        {
            return MOVE_SPEED;
        }
    }

    [SerializeField]
    protected AudioSource audioSource;
    [SerializeField]
    protected float rotateSpeed = ROTATE_SPEED;
    [SerializeField]
    public new Collider collider;

    private Status status = Status.Inactive;
    private Queue<EntityAction> queue;
    private Quaternion initialRotation;

    public enum Status
    {
        Inactive,
        Idle,
        Moving,
        Attacking,
        Dying,
        Dead,
        Length
    }

    private void Start()
    {
        initialRotation = gameObject.transform.rotation;
    }

    protected virtual void Update()
    {
        if (HasAction())
        {
            ExecuteAction();
        }
    }

    public AEntity()
    {
        queue = new Queue<EntityAction>();
    }

    /// <summary>
    /// Clears all actions and resets entity state.
    /// </summary>
    public virtual void ResetEntity()
    {
        status = Status.Idle;
        queue.Clear();
        gameObject.transform.rotation = initialRotation;
    }

    /// <summary>
    /// Clears queued actions.
    /// </summary>
    public void ClearActions()
    {
        queue.Clear();
    }

    /// <summary>
    /// Gets current entity status.
    /// </summary>
    public Status GetStatus()
    {
        return status;
    }

    /// <summary>
    /// Sets the rotation speed of the entity.
    /// </summary>
    public void SetRotateSpeed(float rotateSpeed)
    {
        this.rotateSpeed = rotateSpeed;
    }

    /// <summary>
    /// Deal damage to entity.
    /// </summary>
    /// <param name="damage">Damage dealt.</param>
    public virtual void Damage(int damage)
    {
        if (status == Status.Dying) return;
        Kill();
    }

    /// <summary>
    /// Kills the entity.
    /// </summary>
    public void Kill()
    {
        if (status == Status.Dying) return;
        ScoreBoard.GetInstance().IncrementStreak();
        queue.Clear();
        queue.Enqueue(new EntityAction(Util.Curry2<bool, bool>(KillAction, true), Status.Dying));
        status = Status.Dying;
    }

    /// <summary>
    /// Orders entity to move to <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">A 3D point in the scene.</param>
    public void Move(Vector3 destination)
    {
        if (status == Status.Dying) return;
        queue.Enqueue(new EntityAction(Util.Curry2<Vector3, bool>(MoveAction, destination), Status.Moving));
        status = Status.Moving;
    }

    /// <summary>
    /// Orders entity to rotate and face <paramref name="destination"/> before 
    /// moving to <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">A 3D point in the scene.</param>
    public void RotateAndMove(Vector3 destination)
    {
        if (status == Status.Dying) return;
        queue.Enqueue(new EntityAction(Util.Curry2<Vector3, bool>(RotateAction, destination), Status.Moving));
        queue.Enqueue(new EntityAction(Util.Curry2<Vector3, bool>(MoveAction, destination), Status.Moving));
        status = Status.Moving;
    }

    /// <summary>
    /// Orders entity to rotate to face <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">A 3D point in the scene.</param>
    public void Rotate(Vector3 destination)
    {
        if (status == Status.Dying) return;
        queue.Enqueue(new EntityAction(Util.Curry2<Vector3, bool>(RotateAction, destination), Status.Moving));
        status = Status.Moving;
    }

    /// <summary>
    /// Orders entity to rotate to face <paramref name="destination"/>.
    /// </summary>
    /// <param name="globalRotation">End state of rotation, expressed as a quaternion.</param>
    public void Rotate(Quaternion globalRotation)
    {
        if (status == Status.Dying) return;
        queue.Enqueue(new EntityAction(Util.Curry2<Quaternion, bool>(RotateAction, globalRotation), Status.Moving));
        status = Status.Moving;
    }

    /// <summary>
    /// Orders entity to follow a <see cref="GameObject"/> in the scene by <see cref="AEntity.RotateAndMove(Vector3)"/>
    /// </summary>
    /// <param name="target">Followed by entity.</param>
    public void Follow(GameObject target)
    {
        if (status == Status.Dying) return;
        queue.Clear();
        queue.Enqueue(new EntityAction(Util.Curry2<GameObject, bool>(FollowAction, target), Status.Moving));
        status = Status.Moving;
    }

    /// <summary>
    /// Orders entity to attack another <see cref="AEntity"/>.
    /// </summary>
    /// <param name="entity">Target to be attacked.</param>
    public void Attack(AEntity entity)
    {
        if (status == Status.Dying) return;
        queue.Enqueue(new EntityAction(Util.Curry2<Vector3, bool>(RotateAction, entity.transform.position), Status.Attacking));
        queue.Enqueue(new EntityAction(Util.Curry2<AEntity, bool>(AttackAction, entity), Status.Attacking));
        status = Status.Attacking;
    }

    private void ExecuteAction()
    {
        EntityAction entityAction = queue.Peek();
        status = entityAction.status;
        if (entityAction.func())
        {
            Debug.Log("Entity finished action");
            queue.Dequeue();
            if (status == Status.Dying)
            {
                Debug.Log("Entity died");
                status = Status.Dead;
                queue.Clear();
            }
        }
    }

    private bool HasAction()
    {
        if (status == Status.Inactive || status == Status.Dead)
        {
            return false;
        }
        if (queue.Count == 0)
        {
            status = Status.Idle;
            return false;
        }
        return true;
    }

    protected abstract bool KillAction(bool placeholder);
    protected abstract bool MoveAction(Vector3 destination);
    protected abstract bool RotateAction(Vector3 destination);
    protected abstract bool RotateAction(Quaternion globalRotation);
    protected abstract bool FollowAction(GameObject target);
    protected abstract bool AttackAction(AEntity entity);
}
