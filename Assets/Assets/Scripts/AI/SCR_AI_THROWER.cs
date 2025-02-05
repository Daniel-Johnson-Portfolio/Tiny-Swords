using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AI_THROWER : SCR_AI_CLASS
{

    [SerializeField] private AISettings aiSettings;

    protected override void Start()
    {
        InitializeAISettings(aiSettings);
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
        if (distanceToPlayer <= aiSettings.attackRadiusSize && timeSinceLastAttack >= 0.5f)
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
        if (distanceToPlayer <= (aiSettings.attackRadiusSize / 2))
        {
            agent.ResetPath(); //Stop the AI from moving any closer to the player when in throwing range
        }

        if (distanceToPlayer <= aiSettings.attackRadiusSize && timeSinceLastAttack >= aiSettings.attackCooldown)
        {
            Attack();
            timeSinceLastAttack = 0f;
        }


    }
}
