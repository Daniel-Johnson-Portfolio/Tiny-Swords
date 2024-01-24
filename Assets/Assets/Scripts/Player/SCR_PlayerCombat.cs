using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerCombat : MonoBehaviour
{
    [SerializeField] private float timeSinceLastAttack;
    private const float attackCooldown = 1.21f;

    private float AttackRadiusSize = 2f;
    private Animator animator;
    public bool isAttacking;
    private Vector3 PlayerMovmentDirection;
    [SerializeField] protected SCR_Player_Stats playerStats;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
    }

    private void Update()
    {
        AnimChecker();
        PlayerMovmentDirection = GetComponent<SCR_Player_MasterController>().GetPlayerMovementDirection();
        HandleAttackInput();
        HandleCombat();
        timeSinceLastAttack += Time.deltaTime;
    }

    void HandleAttackInput()
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            isAttacking = false;
        }

        if (timeSinceLastAttack >= attackCooldown && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_ATTACK))
        {
            isAttacking = true;
            timeSinceLastAttack = 0f;
            Attack();
        }
    }

    private void HandleCombat()
    {
        Collider2D[] overlappingColliders = new Collider2D[50];
        int numOverlapping = Physics2D.OverlapCollider(transform.GetComponent<Collider2D>(), new ContactFilter2D(), overlappingColliders);

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Enemy"))
            {
                if (touchingObject.GetComponent<SCR_AI_CLASS>() != null)
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);

                    // Attack if within the AttackRadius
                    if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= attackCooldown && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_ATTACK))
                    {
                        Attack();
                        timeSinceLastAttack = 0f;

                    }
                    if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= 0.5f && isAttacking)
                    {
                        SCR_AI_CLASS aiComponent = touchingObject.GetComponent<SCR_AI_CLASS>();

                        if (aiComponent != null && aiComponent.AICurrentHealth > 0)
                        {
                            aiComponent.DamageAI(1);
                            playerStats.IncrementDamageDealt(1);

                        }
                    }
                }
            }
        }
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AttackFront") && !animator.GetCurrentAnimatorStateInfo(0).IsName("AttackDown"))
        {
            if (Mathf.Abs(PlayerMovmentDirection.x) > Mathf.Abs(PlayerMovmentDirection.y))
            {
                animator.SetBool("AttackFront", true);
            }
            if (Mathf.Abs(PlayerMovmentDirection.x) < Mathf.Abs(PlayerMovmentDirection.y))
            {
                animator.SetBool("AttackUp", PlayerMovmentDirection.y > 0);
                animator.SetBool("AttackDown", PlayerMovmentDirection.y < 0);
            }
            else
            {
                animator.SetBool("AttackFront", true);
            }
        }

    }

    void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsName("Down"))
        {
            animator.SetBool("AttackDown", false);

        }
        if (!currentState.IsName("Up"))
        {
            animator.SetBool("AttackUp", false);

        }

        if (!currentState.IsName("Front"))
        {
            animator.SetBool("AttackFront", false);

        }
    }

}