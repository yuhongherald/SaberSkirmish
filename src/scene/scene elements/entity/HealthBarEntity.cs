using System;

/// <summary>
/// An <see cref="AEntity"/> with a <see cref="HealthBar"/>.
/// </summary>
public abstract class HealthBarEntity : AEntity
{
    public HealthBar healthBar;
    protected int currentHealth = 100;
    protected int totalHealth = 100;

    public override void ResetEntity()
    {
        base.ResetEntity();
        currentHealth = totalHealth;
        healthBar.SetHealth(currentHealth);
    }

    protected virtual void ResetEntityMesh()
    {

    }

    public override void Damage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(Math.Max(currentHealth, 10));
    }
}
