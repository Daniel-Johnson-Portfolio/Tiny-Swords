using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Clicked()
    {
        gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<CircleCollider2D>().enabled = false;

    }

    
}
