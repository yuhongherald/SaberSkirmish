using UnityEngine;
using UnityEngine.UI;

class TestCollision:MonoBehaviour
{
    public AEntity testSubject;
    public Player player;
    public Text debugText;
    public int testDamage = 1;

    void Update()
    {
        if (testSubject.GetStatus() == AEntity.Status.Dead || testSubject.GetStatus() == AEntity.Status.Dead)
        {
            return;
        }
        float collisionDistance = player.CalculateWeaponPenetration(testSubject);
        if (debugText)
        {
            debugText.text = collisionDistance.ToString();
        }
        if (collisionDistance > 0) {
            testSubject.Damage(testDamage);
        }
    }
}
