using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [HideInInspector] public float currentHealth;
    Animator anim;

    public float maxHealth = 100f;

    [SerializeField] private Image EnemyHealthBar;
    private SphereCollider targetCollider;
    public int ExpAmount=10;
    public static event Action<int> onDeath;
    private void Awake()
    {
        currentHealth = maxHealth;
        targetCollider = GetComponentInChildren<SphereCollider>();
        anim = GetComponent<Animator>();
        
    }

    public void  TakeDamage(float amount)
    {
        currentHealth -= amount;
        EnemyHealthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth > 0)
        {
            if (this.gameObject.tag == "Enemy")
            {
                AudioManager.instance.PlaySfx(7);
            }
            anim.SetTrigger("Hit");
        }
        if (currentHealth <= 0)
        {
            
            Canvas canvas = EnemyHealthBar.gameObject.GetComponentInParent<Canvas>();
            onDeath(ExpAmount);
            if (targetCollider.gameObject.activeInHierarchy)
            {
                targetCollider.gameObject.SetActive(false);
            }
            if (canvas.gameObject.activeInHierarchy)
            {
                canvas.gameObject.SetActive(false);
            }
            
        }
    }

    
}
