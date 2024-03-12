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
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> npcs = new List<GameObject>();
    public SCR_Player_Stats playerStats;
    public SCR_PlayerInventory playerInventory;
    [SerializeField] private SCR_Tools tools;

    void Start() 
    {
        tools = FindObjectOfType<SCR_Tools>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        playerInventory = FindObjectOfType<SCR_PlayerInventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            AddToInventory();
        }
        if (npcs.Count == 0) 
        {
            tools.ResetCamera();
        }

    }

    public void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Collectable") && !gameObjects.Contains(Collider.gameObject))
        {
            gameObjects.Add(Collider.gameObject);
        }
        if (Collider.gameObject.layer == LayerMask.NameToLayer("NPC") && !npcs.Contains(Collider.gameObject))
        {
            npcs.Add(Collider.gameObject); //Add the NPC to the list
            
        }
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Collectable"))
        {
           gameObjects.Remove(Collider.gameObject);
        }
        if (Collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            npcs.Remove(Collider.gameObject); //Remove the NPC to the list
        }
    }

    public void AddToInventory()
    {
        if (gameObjects.Count > 0) 
        {
            if (gameObjects.First<GameObject>().name == "Gold(Clone)")
            {
                playerStats.IncrementGoldCollected();
                playerInventory.IncrementGold();
            }
            if (gameObjects.First<GameObject>().name == "Log(Clone)")
            {
                playerStats.IncrementWoodCollected();
                playerInventory.IncrementWood();
            }
            if (gameObjects.First<GameObject>().name == "Meat(Clone)")
            {
                playerStats.IncrementMeatCollected();
                playerInventory.IncrementMeat();
            }
            Destroy(gameObjects.First<GameObject>());
        }

    
    }

}
