using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NPC_CLASS : MonoBehaviour
{
    private bool active;
    private GameObject Banner;
    private GameObject Scroll;
    private SCR_Tools tools;
    private Button AcceptButton;
    private Button DenyButton;
    private bool TransactionComplete;
    private float timer;
    private SCR_TradeGiver TradeGiver;

    // Start is called before the first frame update
    void Start()
    {
        InitializeReferences();
        SubscribeToButtonEvents();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTransactionCooldown();

        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON1) && !TransactionComplete)
        {
            QuestPreview();
            gameObject.GetComponent<Animator>().SetBool("TransactionComplete", false);
        }
    }

    void InitializeReferences()
    {
        timer = 0;
        TransactionComplete = false;
        TradeGiver = FindObjectOfType<SCR_TradeGiver>();
        tools = FindObjectOfType<SCR_Tools>();
        active = false;
        Banner = transform.GetChild(0).gameObject;
        Scroll = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        AcceptButton = Scroll.transform.Find("Accept").GetComponent<Button>();
        DenyButton = Scroll.transform.Find("Deny").GetComponent<Button>();
    }

    void SubscribeToButtonEvents()
    {
        DenyButton.onClick.AddListener(delegate { TradeGiver.DenyReRoll(DenyButton); });
        AcceptButton.onClick.AddListener(delegate { TradeGiver.AcceptTrade(AcceptButton); });
    }

    void HandleTransactionCooldown()
    {
        if (TransactionComplete)
        {
            CloseAll();
            tools.ResetCamera();
            timer += Time.deltaTime;
            if (timer > 1) // 2 minute cooldown
            {
                timer = 0;
                TransactionComplete = false;
            }
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
            ResetUIText();
            tools.ResetCamera();
            active = false;
        }
    }

    void ResetUIText()
    {
        transform.GetChild(0).GetChild(0).GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "";
    }

    public void CloseAll()
    {
        tools.AddToQueue(tools.Close(Scroll));
        tools.AddToQueue(tools.Close(Banner));
        StartCoroutine(tools.ProcessCodeQueue());
    }
}
