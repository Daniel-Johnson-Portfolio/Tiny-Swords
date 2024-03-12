using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoblinHouseSpawner : MonoBehaviour
{

    public int SpawnChance;
    public GameObject[] Goblins;
  
    private void FixedUpdate()
    {
        SpawnChance = Random.Range(0, 1000);

        if (SpawnChance == 50)
        {
            SpawnGoblin();
        }
    }

    void SpawnGoblin() 
    {
        int rand = Random.Range(0, Goblins.Length);
        GameObject GoblinInstance = Instantiate(Goblins[rand], transform.position + new Vector3(0, -2, 0) , Quaternion.identity);
    }
}
