using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using TMPro;

public class SCR_M_UI_Manager : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();
    public SCR_Player_Stats playerStats;

    void Start() 
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            Debug.Log(gameObjects.First<GameObject>().name);
            AddToInventory();
            
        }
    }

    public void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Gold") && !gameObjects.Contains(Collider.gameObject))
        {
            gameObjects.Add(Collider.gameObject);
        }
       
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Gold"))
        {
            gameObjects.Remove(Collider.gameObject);
            
        }
        
    }

    void TheDebug() 
    {
        foreach (GameObject obj in gameObjects) 
        {
            Debug.Log(obj.name);
        
        }

    }

    public void AddToInventory()
    {
        Destroy(gameObjects.First<GameObject>());
        playerStats.IncrementGoldCollected();

    }

}
