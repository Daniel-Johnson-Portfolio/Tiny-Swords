using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public bool PlayerLocked;
    [SerializeField] private SCR_Tools tools;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLocked = true;
        tools = FindObjectOfType<SCR_Tools>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLocked) 
        {
            gameObject.transform.position = player.transform.position + new Vector3(0, 0, -15);
        }
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        if (npcs.Length == 0)
        {
            
            tools.ResetCamera();
        }

            //if no objects with tag NPC are found, loack the camqera to the player and set the otho size to 5
    }
}
