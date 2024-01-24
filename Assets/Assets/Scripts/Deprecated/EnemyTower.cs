using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            try
            {
                AI script = Collider.GetComponent<AI>();
                script.IndividualMoveRadius = 15f;

            }
            catch { }
            try
            {
                AI_TNT script = Collider.GetComponent<AI_TNT>();
                script.IndividualMoveRadius = 15f;

            }
            catch { }
            try
            {
                AI_barrel script = Collider.GetComponent<AI_barrel>();
                script.IndividualMoveRadius = 15f;

            }
            catch { }
        }
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            try
            {
                AI script = Collider.GetComponent<AI>();
                script.MoveRadius = 10f;

            }
            catch { }
            try
            {
                AI_TNT script = Collider.GetComponent<AI_TNT>();
                script.MoveRadius = 10f;

            }
            catch { }
            try
            {
                AI_barrel script = Collider.GetComponent<AI_barrel>();
                script.MoveRadius = 10f;

            }
            catch { }
        }
    }
}
