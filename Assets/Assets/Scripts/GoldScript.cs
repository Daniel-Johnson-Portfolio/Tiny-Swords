using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldScript : MonoBehaviour
{
    private GameObject Banner;
    private bool active;
    private GameObject Player;
    private TextMeshProUGUI tmpText;
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
            Player = Collider.gameObject;
            active = true;
            StartCoroutine(tools.Open(Banner));
        }
    }

    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            active = false;

            StartCoroutine(tools.Close(Banner));
        }
    }
}
