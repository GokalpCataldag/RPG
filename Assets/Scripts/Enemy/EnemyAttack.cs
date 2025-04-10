using UnityEditor.Networking.PlayerConnection;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 10f;
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if(playerHealth != null) 
        { 
            playerHealth.TakeDamage(damage);
        }
    }
}
