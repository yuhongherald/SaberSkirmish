using System;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Linear version of <see cref="HealthBar"/>
/// </summary>
public class LinearHealthBar : HealthBar
{
    public Image healthBar;
    public Image shadowBar;

    public override void SetHealth(int health)
    {
        Debug.Log("Healthbar " + gameObject.name + " new health value: " + health);
        health = Math.Min(Math.Max(health, 0), totalHealth);
        currentHealth = health;
        healthBar.fillAmount = (float)currentHealth / (float)totalHealth;
        Color color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);
        healthBar.color = color;
    }

    public override void Show(bool show)
    {
        gameObject.SetActive(show);
        return;
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (currentMark == currentHealth)
        {
            return;
        }
        int rawMark = currentHealth - currentMark;
        currentMark += Math.Sign(rawMark) * Math.Min(Math.Abs(rawMark),
            (int)((float)totalHealth * depletionRate));
        shadowBar.fillAmount = (float)currentMark / (float)totalHealth;
    }
}
