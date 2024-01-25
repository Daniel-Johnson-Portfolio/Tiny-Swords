using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldScript : MonoBehaviour
{
    private GameObject Banner;
    [SerializeField] public SCR_Tools tools;

    void Start()
    {
        tools = FindObjectOfType<SCR_Tools>();
        Banner = gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }

    public void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            StartCoroutine(tools.Open(Banner));
        }
    }

    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            StartCoroutine(tools.Close(Banner));
        }
    }
}
