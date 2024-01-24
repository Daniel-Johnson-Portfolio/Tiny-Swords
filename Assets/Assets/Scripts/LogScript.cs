using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogScript : MonoBehaviour
{
    private GameObject Banner;
    private bool active;
    private GameObject Player;
    private TextMeshProUGUI tmpText;
    [SerializeField] protected SCR_Player_Stats playerStats;
    [SerializeField] public SCR_Tools tools;

    void Start()
    {
        tools = FindObjectOfType<SCR_Tools>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Banner = gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            AddToInventory();
        }
    }

    public void AddToInventory()
    {
        StartCoroutine(tools.Close(Banner));
        tmpText = Player.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        string originalText = tmpText.text;
        if (int.TryParse(originalText, out int Wood))
        {
            Wood++;
            tmpText.SetText(Wood.ToString());
            playerStats.IncrementWoodCollected();
            Destroy(gameObject);
        }
    }



    public void OnTriggerEnter2D(Collider2D Collider)
    {
        Debug.Log("TriggerStay2D");
        if (Collider.gameObject.tag == "Player")
        {
            Player = Collider.gameObject;
            active = true;
            StartCoroutine(tools.Open(Banner));
        }
    }

    void OnTriggerExit2D(Collider2D Collider)
    {
        Debug.Log("TriggerExit2D");
        if (Collider.gameObject.tag == "Player")
        {
            active = false;

            StartCoroutine(tools.Close(Banner));
        }
    }
}
