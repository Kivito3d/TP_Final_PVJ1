using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [Header("Health Settings")]

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private float deathTimeAnimation = 2.8f;

    [SerializeField] private bool isDead;
    public bool IsDead
    {
        get => isDead;
          
    }

    [SerializeField] private GameObject vfx;

    private Rigidbody rb;
    private Collider col;
    private Attack attack;
    private MeshRenderer mesh;

    private Animator animator;

    public event Action<float> OnChangeHealthPlayer;
    private void Awake() 
    { 
        isDead = false; 
        playerStats.ResetStats();
        enemyStats.ResetStats();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        attack = GetComponent<Attack>();
        mesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        animator.SetTrigger("GetHit");
        if (gameObject.CompareTag("Player"))
        {
            playerStats.currentHealth -= amount;
            OnChangeHealthPlayer?.Invoke(playerStats.currentHealth);

            if (playerStats.currentHealth <= 0f)
            {
                Dead();
                return;
            }
        }

        if (gameObject.CompareTag("Character"))
        {
            enemyStats.currentHealth -= amount;

            if (enemyStats.currentHealth <= 0f)
            {
                Dead();
                return;
            }
        }


    }
    private void Dead() 
    { 
        isDead = true;

        animator.SetTrigger("Death");

        if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance.NotifyPlayerDead();
        }
        else if(gameObject.CompareTag("Character"))
        {
            GameManager.Instance.NotifyEnemyDefeat();
        }

        //vfx.SetActive(true);
        StartCoroutine(DestroyTransition());


        //Debug.Log("Character eliminado"); 

    }

    IEnumerator DestroyTransition()
    {
        //reproducir animacion
        col.enabled = false;
        rb.isKinematic = true;
        
        attack.enabled = false;

        yield return new WaitForSeconds(deathTimeAnimation);
        mesh.enabled = false;

        Destroy(this.gameObject);
    }

}
