using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public bool PlayerLocked;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLocked) 
        {
            gameObject.transform.position = player.transform.position + new Vector3(0, 0, -15);
        }
       
        
    }
}
