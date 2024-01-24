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
    [SerializeField] protected SCR_Player_Stats playerStats;
    [SerializeField] public SCR_Tools tools;
    public SCR_M_UI_Manager UI_Manager;
    public delegate void TheDelegate();
    public TheDelegate theDelegate;

    void Start()
    {
        UI_Manager = FindObjectOfType<SCR_M_UI_Manager>();
        theDelegate = AddToInventory;
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
        tmpText = Player.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        string originalText = tmpText.text;
        if (int.TryParse(originalText, out int Gold))
        {
            Gold++;
            tmpText.SetText(Gold.ToString());
            playerStats.IncrementGoldCollected();
            Destroy(gameObject);
        }
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
