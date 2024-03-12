using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT_Script : MonoBehaviour
{
    [SerializeField] Transform Target;
    Animator animator;
    bool exploded = false;
    float TNT_Speed = 7.0f;
    float rotationSpeed = 200.0f;
    [SerializeField] protected SCR_Player_Stats playerStats;

    Vector2 initialPlayerPosition;

    void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        initialPlayerPosition = Target.position;
    }

    void Update()
    {
        if (!exploded)
        {
            MoveTowardsPosition(initialPlayerPosition);
        }
    }

    void MoveTowardsPosition(Vector2 targetPosition)
    {
        Vector2 TNT_Direction = (targetPosition - (Vector2)transform.position).normalized;

        transform.Translate(TNT_Direction * TNT_Speed * Time.deltaTime);

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.1f)
        {
            exploded = true;
            animator.SetBool("Explode", true);

            transform.Translate(new Vector3(0, -2, 0));
            transform.localScale = transform.localScale + new Vector3(2, 2, 2);

    
            Collider2D[] overlappingColliders = new Collider2D[50];
            ContactFilter2D contactFilter = new ContactFilter2D();
            int numOverlapping = Physics2D.OverlapCollider(GetComponent<Collider2D>(), contactFilter, overlappingColliders);

            for (int i = 0; i < numOverlapping; i++)
            {
                Collider2D touchingCollider = overlappingColliders[i];
                GameObject touchingObject = touchingCollider.gameObject;

                if (touchingObject.GetComponent<SCR_Player_MasterController>() != null)
                {
                    SCR_Player_MasterController playerController = touchingObject.GetComponent<SCR_Player_MasterController>();
                    playerController.CurrentHealth -= 200;
                                                           
                }
            }

            Destroy(gameObject, 0.5f);
        }
    }

}
