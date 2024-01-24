using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_THROWER : SCR_AI_CLASS
{
    protected override float AttackRadiusSize
    {
        get { return 5f; }
    }
    protected override float attackCooldown
    {
        get { return 5f; }
    }

    protected override void Start()
    {
        base.Start(); 
        
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void Attack()
    {
        animator.SetBool("Throw", true);

        Vector3 aiPosition = transform.position + new Vector3(0, 1, 0);
        if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= 0.5f)
        {
            GameObject DynamiteInstance = Instantiate(Resources.Load<GameObject>("Dynamite"), aiPosition, Quaternion.identity);
        }
    }

    protected override void AnimChecker()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        if (!currentState.IsName("Throw"))
        {
            animator.SetBool("Throw", false);
        }

    }
    protected override void PlayerSpotted()
    {
        if (distanceToPlayer <= (AttackRadiusSize / 2))
        {
            agent.ResetPath();
        }

        if (distanceToPlayer <= AttackRadiusSize && timeSinceLastAttack >= attackCooldown)
        {
            Attack();
            timeSinceLastAttack = 0f;
        }


    }


    // Override other methods as needed
}
