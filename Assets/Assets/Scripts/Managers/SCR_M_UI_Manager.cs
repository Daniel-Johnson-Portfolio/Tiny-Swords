using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SCR_M_UI_Manager : MonoBehaviour, IInterface
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public void GetItem()
    {
        throw new NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Gold") && !gameObjects.Contains(Collider.gameObject))
        {
            gameObjects.Add(Collider.gameObject);
        }
        TheDebug();
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.CompareTag("Gold"))
        {
            gameObjects.Remove(Collider.gameObject);
            
        }
        TheDebug();
    }

    void TheDebug() 
    {
        foreach (GameObject obj in gameObjects) 
        {
            Debug.Log(obj.name);
        
        }

        var chosen = gameObjects.First<GameObject>();
        var chosen2 = gameObjects.Last<GameObject>();

    }
}

public interface IInterface
{
    public void GetItem();
}
