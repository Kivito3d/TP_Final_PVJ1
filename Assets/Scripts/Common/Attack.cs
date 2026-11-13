using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{

    private WeaponHitBox hitBox;

    [SerializeField] private float cooldownAttack;
    private bool canAttack = true;

    [SerializeField] private bool isPlayer;

    private PlayerMovement pM;

    private Animator animator;

    private void Awake()
    {
        hitBox = GetComponent<WeaponHitBox>();
        animator = GetComponent<Animator>();
        pM = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
            {
                Melee();     
  
            }
        }
    }



    public void Melee()
    {
        if (!canAttack) return;
        animator.SetTrigger("Attack");
        StartCoroutine(AttackCooldown());
    }

    public void EnableHitbox()
    {
        if (hitBox != null) hitBox.EnableHit();
    }

    public void DisableHitbox()
    {
        if (hitBox != null) hitBox.DisableHit();
    }

    IEnumerator AttackCooldown()
    {
        float currentSpeed=0f;
        canAttack = false;

        if (isPlayer)
        {
            currentSpeed = pM.Speed;
            pM.Speed = 0f;

            yield return new WaitForSeconds(cooldownAttack);


            pM.Speed = currentSpeed;
            canAttack = true;
        }
    }


}
