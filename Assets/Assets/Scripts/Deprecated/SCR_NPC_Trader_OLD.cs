using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_NPC_Trader_OLD : MonoBehaviour
{
    [SerializeField] private bool active;
    [SerializeField] private GameObject Banner;
    [SerializeField] private GameObject Scroll;
    [SerializeField] public SCR_Tools tools;
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Button DenyButton;
    public bool TransactionComplete;
    private float timer;
    [SerializeField] private SCR_TradeGiver TradeGiver;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        TransactionComplete = false;
        TradeGiver = FindObjectOfType<SCR_TradeGiver>();
        tools = FindObjectOfType<SCR_Tools>();
        active = false;
        Banner = transform.GetChild(0).gameObject;
        Scroll = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        AcceptButton = Scroll.transform.GetChild(4).transform.GetComponent<Button>();
        DenyButton = Scroll.transform.GetChild(5).transform.GetComponent<Button>();
        DenyButton.onClick.AddListener(delegate { TradeGiver.DenyReRoll(DenyButton); });
        AcceptButton.onClick.AddListener(delegate { TradeGiver.AcceptTrade(AcceptButton); });
    }

    // Update is called once per frame
    void Update()
    {
        if (TransactionComplete) 
        {
            CloseAll();
            tools.ResetCamera();
            timer += Time.deltaTime;
            if (timer > 1) //2 minute cooldown
            {
                timer = 0;
                TransactionComplete = false;
            }
        }


        if (active == true && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON1) && !TransactionComplete)
        {
            //Banner.SetActive(false);
            QuestPreview();
            transform.gameObject.GetComponent<Animator>().SetBool("TransactionComplete", false);
        }

    }
    void QuestPreview()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Scroll.SetActive(true);

        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        Camera myCamera = Camera.main;
        myCamera.orthographicSize = 2;
        myCamera.GetComponent<CameraScript>().PlayerLocked = false;
        myCamera.transform.position = Banner.transform.position + new Vector3(-0.5f, 0.5f, -15f);
        StartCoroutine(tools.Open(Scroll));
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player" && !TransactionComplete)
        {
            
            Banner.SetActive(true);
            active = true;
            StartCoroutine(tools.Open(Banner));
            Collider.gameObject.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);

        }
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player" && !TransactionComplete)
        {
            CloseAll();
            transform.GetChild(0).GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "";
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            tools.ResetCamera();
            active = false;

        }
    }

    public void CloseAll()
    {
        tools.AddToQueue(tools.Close(Scroll));
        tools.AddToQueue(tools.Close(Banner));
        StartCoroutine(tools.ProcessCodeQueue());

    }
}
