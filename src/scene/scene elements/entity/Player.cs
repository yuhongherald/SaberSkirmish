using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An entity class which players control.
/// </summary>
public class Player : HealthBarEntity
{

    [SerializeField]
    private GameObject weaponPoint;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private Collider weaponCollider;
    [SerializeField]
    private float weaponLength = 1f;

    [SerializeField]
    private GameObject playerLeft;
    [SerializeField]
    private GameObject playerRight;
    [SerializeField]
    private GameObject playerFront;
    [SerializeField]
    private GameObject playerNeutral;
    [SerializeField]
    private Text debugText;

    public const float MIN_ANGULAR_SPEED = 0.7f;
    public const float MAX_ANGULAR_SPEED = 2.1f;
    private const float RANGED_ATTACK_DISTANCE = 8.0f;
    private const float WEAPON_OFFSET_FACTOR = 0.1f;
    private const int WEAPON_SPEED_DECAY_FACTOR = 3;
    private const int HALF_ROUND_ROTATION = 180;
    private const int FULL_ROUND_ROTATION = 360;
    private const int WEAPON_LEFT_THRESHOLD = 70;
    private const int WEAPON_RIGHT_THRESHOLD = -70;
    private const int WEAPON_CENTER_THRESHOLD = 20;
    private Position controllerPosition = Position.front;
    private Vector3 weaponDestination;
    private float weaponTime;
    private float weaponSpeed;
    private float weaponAngularSpeed = 0;

    public Quaternion OffsetRotation { get; set; }
    public int PlayerActiveTimer { get; set; }
    public float LastActive { get; set; }

    public override float MoveSpeed
    {
        get
        {
            return 0.45f;
        }
    }

    public enum Position
    {
        neutral,
        left,
        right,
        front,
        farLeft,
        farRight,
        farFront,
        length
    }

    protected override void Update()
    {
        base.Update();
        CheckControlsWeaponInput();
        UpdateWeaponPosition();
        MoveWeapon();
    }

    private void CheckControlsWeaponInput()
    {
        if (Input.GetKey("left"))
        {
            Vector3 rotation = weapon.transform.eulerAngles;
            rotation.z = 90;
            weapon.transform.eulerAngles = rotation;
        }
        else if (Input.GetKey("right"))
        {
            Vector3 rotation = weapon.transform.eulerAngles;
            rotation.z = -90;
            weapon.transform.eulerAngles = rotation;
        }
        else if (Input.GetKey("up"))
        {
            Vector3 rotation = weapon.transform.eulerAngles;
            rotation.z = 0;
            weapon.transform.eulerAngles = rotation;
        }
        if (Input.GetKey("space"))
        {
            Vector3 rand = UnityEngine.Random.onUnitSphere;
            OffsetRotation = new Quaternion(rand.x, rand.y, rand.z, 0);
            UpdateWeaponPointRotation(weaponPoint.transform.localRotation * Quaternion.Euler(0, -20, 0));
        }
        else
        {
            UpdateWeaponPointRotation(weaponPoint.transform.localRotation);
        }
    }

    public GameObject GetWeaponPoint()
    {
        return weaponPoint;
    }

    public float GetWeaponLength()
    {
        return weaponLength;
    }

    public float GetWeaponAngularSpeed()
    {
        return weaponAngularSpeed;
    }

    public Position GetControllerPosition()
    {
        return controllerPosition;
    }

    public Vector3 GetPosition(Position position)
    {
        switch (position)
        {
            case Position.left:
                return GetLeft().transform.position;
            case Position.right:
                return GetRight().transform.position;
            case Position.front:
                return GetFront().transform.position;
            case Position.farLeft:
                return GetFarLeft();
            case Position.farRight:
                return GetFarRight();
            case Position.farFront:
                return GetFarFront();
            default:
                return gameObject.transform.position;
        }
    }

    public GameObject GetLeft()
    {
        return playerLeft;
    }

    public GameObject GetRight()
    {
        return playerRight;
    }

    public GameObject GetFront()
    {
        return playerFront;
    }

    public Vector3 GetFarLeft()
    {
        Vector3 direction = (playerLeft.transform.position - transform.position);
        return direction * (RANGED_ATTACK_DISTANCE / direction.magnitude) + playerLeft.transform.position;
    }

    public Vector3 GetFarFront()
    {
        Vector3 direction = (playerFront.transform.position - transform.position);
        return direction * (RANGED_ATTACK_DISTANCE / direction.magnitude) + playerFront.transform.position;
    }

    public Vector3 GetFarRight()
    {
        Vector3 direction = (playerRight.transform.position - transform.position);
        return direction * (RANGED_ATTACK_DISTANCE / direction.magnitude) + playerRight.transform.position;
    }

    /// <summary>
    /// Calculates how deep the weapon penetrated <paramref name="entity"/>.
    /// </summary>
    public float CalculateWeaponPenetration(AEntity entity)
    {
        if (weaponAngularSpeed < MAX_ANGULAR_SPEED || entity.GetStatus() == Status.Dying || entity.GetStatus() == Status.Dead)
        {
            return 0;
        }
        Vector3 collisionDirection;
        float distance;
        Physics.ComputePenetration(entity.collider, entity.transform.position, entity.transform.rotation,
            weaponCollider, weaponPoint.transform.position, weaponPoint.transform.rotation, out collisionDirection, out distance);
        return distance;
    }

    /// <summary>
    /// Resets the position of the player's weapon.
    /// </summary>
    public void ResetWeapon()
    {
        weaponTime = 0;
        weaponSpeed = 0;
        Vector3 offset = (GetRight().transform.localPosition - playerNeutral.transform.localPosition);
        offset.Scale(Vector3.one * WEAPON_OFFSET_FACTOR);
        weaponPoint.transform.localPosition = GetFront().transform.localPosition - offset;
        weaponDestination = weaponPoint.transform.localPosition;
        controllerPosition = Position.front;
    }

    /// <summary>
    /// Sets the player's weapon rotation to <paramref name="newRotation"/>.
    /// </summary>
    public void SetWeaponRotation(Quaternion newRotation)
    {
        weaponAngularSpeed = 0.15f * weaponAngularSpeed + 0.85f * Quaternion.Angle(weapon.transform.localRotation, newRotation);
        weapon.transform.localRotation = newRotation;

        audioSource.volume = Mathf.Min((weaponAngularSpeed - MIN_ANGULAR_SPEED) / (MAX_ANGULAR_SPEED - MIN_ANGULAR_SPEED), 1) * 0.6f;
    }

    private void UpdateWeaponPosition()
    {
        Vector3 angles = weapon.transform.localRotation.eulerAngles;
        float z = angles.z;
        if (z >= HALF_ROUND_ROTATION)
        {
            z = z - FULL_ROUND_ROTATION;
        }
        debugText.text = z.ToString();
        if (z > WEAPON_LEFT_THRESHOLD && controllerPosition != Position.left)
        {
            SetWeaponPoint(Position.left);
        } else if (z < WEAPON_RIGHT_THRESHOLD && controllerPosition != Position.right)
        {
            SetWeaponPoint(Position.right);
        }
        else if (Mathf.Abs(z) < WEAPON_CENTER_THRESHOLD && controllerPosition != Position.front)
        {
            SetWeaponPoint(Position.front);
        }
    }

    private void UpdateWeaponPointRotation(Quaternion newRotation)
    {
        weaponAngularSpeed = 0.15f * weaponAngularSpeed + 0.85f *
            Quaternion.Angle(weaponPoint.transform.localRotation, newRotation);
        weaponPoint.transform.localRotation = newRotation;

        audioSource.volume = Mathf.Min((weaponAngularSpeed - MIN_ANGULAR_SPEED) /
            (MAX_ANGULAR_SPEED - MIN_ANGULAR_SPEED), 1) * 0.6f;
    }

    private void SetWeaponPoint(Position controllerPosition)
    {
        if (controllerPosition.Equals(this.controllerPosition) ||
            weaponAngularSpeed >= MIN_ANGULAR_SPEED)
        {
            return;
        }
        this.controllerPosition = controllerPosition;
        Vector3 offset;
        switch (controllerPosition)
        {
            case Position.right:
                offset = (GetRight().transform.localPosition - playerNeutral.transform.localPosition);
                offset.Scale(new Vector3(WEAPON_OFFSET_FACTOR,
                    WEAPON_OFFSET_FACTOR, WEAPON_OFFSET_FACTOR));
                weaponDestination = GetRight().transform.localPosition - offset;
                break;
            case Position.left:
                offset = (GetLeft().transform.localPosition - playerNeutral.transform.localPosition);
                offset.Scale(new Vector3(WEAPON_OFFSET_FACTOR,
                    WEAPON_OFFSET_FACTOR, WEAPON_OFFSET_FACTOR));
                weaponDestination = GetLeft().transform.localPosition - offset;
                break;
            case Position.front:
                offset = (GetFront().transform.localPosition - playerNeutral.transform.localPosition);
                offset.Scale(new Vector3(WEAPON_OFFSET_FACTOR,
                    WEAPON_OFFSET_FACTOR, WEAPON_OFFSET_FACTOR));
                weaponDestination = GetFront().transform.localPosition - offset;
                break;
            default:
                return;
        }

        weaponTime = 0.5f;
        weaponSpeed = (weaponDestination - weaponPoint.transform.localPosition).magnitude / 0.1f;
    }

    private void MoveWeapon()
    {
        if (weaponTime <= 0 || weaponPoint.transform.localPosition.Equals(weaponDestination))
        {
            return;
        }
        weaponTime -= Time.deltaTime;
        weaponTime = Mathf.Max(weaponTime, 0);
        weaponPoint.transform.localPosition =
            Vector3.MoveTowards(weaponPoint.transform.localPosition,
            weaponDestination, weaponSpeed * Time.deltaTime);
        if (weaponTime * weaponSpeed > WEAPON_SPEED_DECAY_FACTOR *
            (weaponPoint.transform.localPosition - weaponDestination).magnitude)
        {
            weaponSpeed /= WEAPON_SPEED_DECAY_FACTOR;
        }
    }

    protected override bool KillAction(bool placeholder)
    {
        Debug.LogWarning("Player has just been killed!");
        return true;
    }

    protected override bool AttackAction(AEntity entity)
    {
        entity.Kill();
        return true;
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
            return true;
        }
        else
        {
            gameObject.transform.position =
                gameObject.transform.position + movement.normalized * MoveSpeed;
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
            gameObject.transform.rotation =
                Quaternion.Lerp(gameObject.transform.rotation, globalRotation,
                rotateSpeed / angle);
            return false;
        }
    }
}
