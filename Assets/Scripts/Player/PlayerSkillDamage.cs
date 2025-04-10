using UnityEngine;

public class PlayerSkillDamage : MonoBehaviour
{

    public LayerMask enemyLayer;
    public float radius = 0.5f;
    public float damgeCount = 10f;

    private EnemyHealth enemyHealth;
    protected bool colided;

    internal virtual void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider hit in hits)
        {
            enemyHealth = hit.gameObject.GetComponent<EnemyHealth>();
            colided = true;
        }
        if (colided)
        {
            enemyHealth.TakeDamage(damgeCount);
            enabled = false;
        }
    }
}
