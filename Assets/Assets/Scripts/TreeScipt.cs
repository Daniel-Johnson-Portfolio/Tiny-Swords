using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public int TreeHealth;
    public float damageCooldown = 1.0f; // Adjust the cooldown time as needed
    public float timeSinceLastDamage = 0f;
    private Animator Animator;
    [SerializeField] private Collider2D myCollider;
    public float HitTime;
    public LayerMask layersToCheck;
    


    void Start()
    {
        
        Animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        TreeHealth = 5;
    }

    void Update()
    {
        if (TreeHealth > 0) 
        {
            Collider2D[] overlappingColliders = new Collider2D[1];

            int numOverlapping = Physics2D.OverlapCollider(myCollider, new ContactFilter2D(), overlappingColliders);
            timeSinceLastDamage += Time.deltaTime;
            HitTime = HitTime + 0.01f;
            if (HitTime > 1f)
            {
                HitTime = 0;
                Animator.SetBool("Hit", false);
            }

            if (numOverlapping > 0)
            {
                Collider2D touchingCollider = overlappingColliders[0];
                GameObject touchingObject = touchingCollider.gameObject;
                SCR_PlayerCombat playerController = touchingObject.GetComponent<SCR_PlayerCombat>();
                if (touchingObject.tag == "Player" && playerController.isAttacking && timeSinceLastDamage >= damageCooldown)
                {
                    Animator.SetBool("Hit", true);
                    TreeHealth--;
                    timeSinceLastDamage = 0f;
                }
            }
        }
        if (TreeHealth == 0) 
        {
            Animator.SetBool("Alive", false);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            myCollider.enabled = false;
            //gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            
            Vector3 Position = transform.position;
            GameObject WoodInstance = Instantiate(Resources.Load<GameObject>("Log"), Position + new Vector3(0,1,0), Quaternion.identity);
            gameObject.GetComponent<TreeScript>().enabled = false;


        }

    }
}

