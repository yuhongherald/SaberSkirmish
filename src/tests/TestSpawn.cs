using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public int spawnCount = 20;
    private void Start()
    {
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        Animator newAnim;
        Vector2 random;
        for (int i = 0; i < 20; i++)
        {
            newAnim = Animator.Instantiate(anim);
            random = Random.insideUnitCircle * 10;
            newAnim.transform.localPosition = new Vector3(random.x, -4, random.y + 10);
            newAnim.transform.parent = this.transform;
        }
    }
}
