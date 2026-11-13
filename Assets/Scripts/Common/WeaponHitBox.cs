using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{

    [Header("Damage Settings")]
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private PlayerStats playerStats;
    private float damage;
    [SerializeField] private string targetTag = "Character";

    [SerializeField]private CapsuleCollider hitbox;
    
    //private readonly HashSet<GameObject> hitThisSwing = new HashSet<GameObject>();

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    private void Awake()
    {
        if (targetTag != "Player")
        {
            damage = enemyStats.damage;
        }
        else
        {
            damage = playerStats.damage;
        }
        hitbox.isTrigger = true;
        hitbox.enabled = false;   


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitbox.enabled) return;                       
        if (!other.CompareTag(targetTag)) return;         
        //if (hitThisSwing.Contains(other.gameObject)) return; // 1 solo daño por swing

        HealthSystem health = other.GetComponent<HealthSystem>();
        if (health != null && !health.IsDead)
        {
            health.TakeDamage(damage);
            //hitThisSwing.Add(other.gameObject);
        }
    }

    public void EnableHit()
    {
        //hitThisSwing.Clear();     
        hitbox.enabled = true;
    }

    public void DisableHit()
    {
        hitbox.enabled = false;
        //hitThisSwing.Clear();
    }
}


