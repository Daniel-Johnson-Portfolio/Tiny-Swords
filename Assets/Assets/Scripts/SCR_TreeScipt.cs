using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public int TreeHealth;
    public float damageCooldown = 1.0f;
    public float timeSinceLastDamage = 0f;
    private Animator Animator;
    [SerializeField] private Collider2D myCollider;
    public float HitTime;
    public LayerMask layersToCheck;

    private Collider2D[] overlappingColliders = new Collider2D[10]; // Adjust size as needed

    void Start()
    {
        Animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        TreeHealth = 5;
        layersToCheck = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (TreeHealth <= 0) DestroyTree();
        else CheckForDamage();
    }

    void CheckForDamage()
    {
        timeSinceLastDamage += Time.deltaTime;
        HitTime += Time.deltaTime;
        if (HitTime > 1f)
        {
            HitTime = 0;
            Animator.SetBool("Hit", false);
        }

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layersToCheck);
        filter.useLayerMask = true;
        filter.useTriggers = false;

        int numOverlapping = Physics2D.OverlapCollider(myCollider, filter, overlappingColliders);

        if (numOverlapping > 0 && timeSinceLastDamage >= damageCooldown)
        {
            foreach (var collider in overlappingColliders)
            {
                if (collider == null) break;
                SCR_PlayerCombat playerController = collider.GetComponent<SCR_PlayerCombat>();
                if (playerController != null && playerController.isAttacking)
                {
                    TakeDamage();
                    break;
                }
            }
        }
    }


    void TakeDamage()
    {
        Animator.SetBool("Hit", true);
        TreeHealth--;
        timeSinceLastDamage = 0f;
    }

    void DestroyTree()
    {
        Animator.SetBool("Alive", false);
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        myCollider.enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        Instantiate(Resources.Load<GameObject>("Log"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        this.enabled = false;
    }

}

