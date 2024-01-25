using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_BOMBER : SCR_AI_CLASS
{
    private bool Exploded = false;
    [SerializeField] private float timeSinceTrigger = 0f;
    [SerializeField] bool Trigger = false;
    protected override int MaxHealth
    {
        get { return 1000; }
    }
    protected override float attackCooldown
    {
        get { return 2; }
    }
    protected override float AttackRadiusSize
    {
        get { return 1f; }
    }


    protected override void Start()
    {
        base.Start(); 
    }

    protected override void Update()
    {
        base.Update();
        if (Trigger) 
        {
            timeSinceTrigger += Time.deltaTime;
            Attack();
        }
    }

    protected override void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Explosion"))
        {
            gameObject.transform.localScale = new Vector3(2, 2, 2);
            Exploded = true;
            DamagePlayer();
        }

    }
    protected override void PlayerSpotted()
    {
        if (distanceToPlayer <= AttackRadiusSize)
        {
            Attack();
            
        }

        if (timeSinceTrigger >= attackCooldown && Trigger == true)
        {
            animator.SetBool("Trigger", false);
        }

    }

    protected override void Attack()
    {
        if (timeSinceTrigger > 2)
        {
            animator.SetBool("Trigger", false);
            Trigger = false;

        }
        else 
        {
            animator.SetBool("Trigger", true);
            Trigger = true;
            base.shouldMoveRandomly = false;
            agent.speed = 0;
            agent.ResetPath();

        }
    }

    protected override void HealthCheck()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (Exploded == true && !currentState.IsName("Explosion")) 
        {
            Vector3 aiPosition = transform.position;
            GameObject goldInstance = Instantiate(Resources.Load<GameObject>("Gold"), aiPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void DamagePlayer()
    {
        Collider2D[] overlappingColliders = new Collider2D[50];
        ContactFilter2D contactFilter = new ContactFilter2D();
        int numOverlapping = Physics2D.OverlapCollider(agent.GetComponent<Collider2D>(), contactFilter, overlappingColliders);

        for (int i = 0; i < numOverlapping; i++)
        {
            Collider2D touchingCollider = overlappingColliders[i];
            GameObject touchingObject = touchingCollider.gameObject;

            if (touchingObject.CompareTag("Player"))
            {
                SCR_Player_MasterController playerController = touchingObject.GetComponent<SCR_Player_MasterController>();
                if (playerController != null)
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);
                    if (distanceToPlayer <= (AttackRadiusSize * 2))
                    {
                        playerController.CurrentHealth -= 10;
                        playerStats.IncrementDamageTaken(2);
                    }
                }
            }
            else if (touchingObject.CompareTag("Enemy"))
            {
                    float distanceToPlayer = Vector2.Distance(transform.position, touchingObject.transform.position);
                  
                    if (distanceToPlayer <= (AttackRadiusSize * 2))
                    {
                        SCR_AI_CLASS aiComponent = touchingObject.GetComponent<SCR_AI_CLASS>();

                        if (aiComponent != null && aiComponent.AICurrentHealth > 0)
                        {
                            aiComponent.DamageAI(1);
                        }
                    }
            }

        }
    }

}
