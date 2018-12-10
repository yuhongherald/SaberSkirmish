using UnityEngine;

/// <summary>
/// Represents a laser projectile shot by an AEntity.
/// </summary>
public class Laser : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 destination;
    private AEntity source;
    private AEntity target;

    private const float SPEED = 0.003f;
    private const int DAMAGE_AMOUNT = 10;

    private int frame = 0;
    private int mult = 1;

    /// <summary>
    /// Tries to deflect the laser using the player's weapon.
    /// If successful, reverses the direction which the laser is travelling.
    /// </summary>
    public void AttemptToDeflectLaser(Player player)
    {
        if (HasCollidedWithWeapon(player))
        {
            Debug.Log(player.name + " deflected laser from " + source.name);
            mult *= -1;
        }
    }

    public void SetSource(AEntity source)
    {
        this.source = source;
    }

    /// <summary>
    /// Directs the laser to go from the source to the target.
    /// </summary>
    public void AimLaser(Vector3 direction, Vector3 destination, AEntity target)
    {
        this.direction = direction;
        this.destination = destination;
        this.target = target;
    }

    /// <summary>
    /// Checks if the laser collided with the given player's weapon.
    /// </summary>
    private bool HasCollidedWithWeapon(Player player)
    {
        if (!isActiveAndEnabled || player.GetWeaponAngularSpeed() < Player.MAX_ANGULAR_SPEED || mult < 0)
        {
            return false;
        }
        Vector3 position = player.GetWeaponPoint().transform.position;
        float distance = (position.x - transform.position.x) * (position.x - transform.position.x)
            + (position.z - transform.position.z) * (position.z - transform.position.z);
        return distance < 4 * player.GetWeaponLength() * player.GetWeaponLength();
    }

    private void Start()
    {
        ;
    }

    private void Update()
    {
        //laser collided with source
        if (frame < 0)
        {
            Debug.Log(source.name + " was killed by own laser.");
            source.ClearActions();
            source.Kill();
            frame = 0;
            gameObject.SetActive(false);
            return;
        }
        //laser collided with target
        if (Vector3.Dot(destination - transform.position, direction) <= 0)
        {
            Debug.Log(source.name + "'s laser damaged " + target.name + " for " + DAMAGE_AMOUNT + " points of damage.");
            target.Damage(DAMAGE_AMOUNT);
            frame = 0;
            gameObject.SetActive(false);
        }
        else
        {
            //move laser
            frame += mult;
            transform.position += mult * direction;
        }
    }
}
