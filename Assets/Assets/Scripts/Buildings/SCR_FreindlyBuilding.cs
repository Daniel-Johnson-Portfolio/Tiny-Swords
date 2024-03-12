using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FreindlyBuilding : MonoBehaviour
{
    private bool InArea;
    private Collider2D collider;

    // Update is called once per frame
    void Update()
    {
        if (InArea == true) 
        {
            if (collider.GetComponent<SCR_Player_MasterController>().CurrentHealth < 1000)
            {
                collider.GetComponent<SCR_Player_MasterController>().CurrentHealth++;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            collider = Collider;
            InArea = true;
        }
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            InArea = false;
        }
    }

}
