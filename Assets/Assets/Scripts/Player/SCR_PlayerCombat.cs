using System.Collections.Generic;
using UnityEngine;

public class SCR_PlayerCombat : MonoBehaviour
{
    [SerializeField] private float timeSinceLastAttack;
    private const float attackCooldown = 1.21f;
    private float attackRadiusSize = 2f;
    private Animator animator;
    public bool isAttacking;
    private Vector2 playerMovementDirection;
    [SerializeField] private SCR_Player_Stats playerStats;
    private SCR_Player_MasterController playerMasterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        playerMasterController = GetComponent<SCR_Player_MasterController>();
    }

    void Update()
    {
        AnimChecker();
        playerMovementDirection = playerMasterController.GetPlayerMovementDirection();
        HandleAttackInput();
        if (isAttacking) HandleCombat();
        timeSinceLastAttack += Time.deltaTime;
    }

    void HandleAttackInput()
    {
        if (!isAttacking && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_ATTACK) && playerMasterController.IsAlive == true)
        {
            isAttacking = true;
            timeSinceLastAttack = 0f;
            Attack();
        }
        else if (timeSinceLastAttack >= attackCooldown)
        {
            isAttacking = false;
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

                    if (distanceToPlayer <= attackRadiusSize && timeSinceLastAttack >= attackCooldown && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_ATTACK))
                    {
                        Attack();
                        timeSinceLastAttack = 0f;

                    }
                    if (distanceToPlayer <= attackRadiusSize && timeSinceLastAttack >= 0.5f && isAttacking)
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
        animator.SetBool("AttackFront", Mathf.Abs(playerMovementDirection.x) >= Mathf.Abs(playerMovementDirection.y));
        animator.SetBool("AttackUp", playerMovementDirection.y > 0 && Mathf.Abs(playerMovementDirection.y) > Mathf.Abs(playerMovementDirection.x));
        animator.SetBool("AttackDown", playerMovementDirection.y < 0 && Mathf.Abs(playerMovementDirection.y) > Mathf.Abs(playerMovementDirection.x));
    }

    // Consider removing AnimChecker if you can handle animation transitions and conditions within your Animator Controller.


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