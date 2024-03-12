using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NPC_CLASS : MonoBehaviour
{
    [SerializeField] protected SCR_Tools tools;
    [SerializeField] private SCR_QUEST_GIVER questGiver;
    

    private bool active;
    [SerializeField] private GameObject banner;
    private GameObject scroll;
    private bool closingInProgress = false;
    protected Button acceptButton;
    protected Button denyButton;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InitializeNPC();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleInteractionInput();
    }

    protected virtual void SetButtons()
    {
        questGiver = FindObjectOfType<SCR_QUEST_GIVER>();
        denyButton.onClick.AddListener(() => questGiver?.HandleDenyReRoll(denyButton));
        acceptButton.onClick.AddListener(() => questGiver?.HandleAccept(acceptButton));
    }

    protected virtual void InitializeNPC()
    {
        
        tools = FindObjectOfType<SCR_Tools>();
        active = false;
        banner = transform.GetChild(0).gameObject;
        scroll = banner.transform.GetChild(0).GetChild(1).gameObject;
        acceptButton = scroll.transform.Find("Accept").GetComponent<Button>();
        denyButton = scroll.transform.Find("Deny").GetComponent<Button>();
        SetButtons();
    }

    protected virtual void HandleInteractionInput()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON1))
        {
            CloseInteraction();
            QuestPreview();
            transform.GetComponent<Animator>().SetBool("QuestComplete", false);
        }
    }

    protected virtual void QuestPreview()
    {
        scroll.SetActive(true);
        tools.SetCamera(banner.transform.position + new Vector3(-0.5f, 0.5f, -15f));
        StartCoroutine(tools.Open(scroll)); 
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.GetComponent<SCR_Player_MasterController>().quest.IsActive == false && !active)
        {
            StartCoroutine(tools.Close(scroll, true));
            if (!active) 
            {
                ReadyBanner();
                collider.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
            }
            
        }
    }

    protected virtual void ReadyBanner() 
    {
        banner.GetComponent<SpriteRenderer>().enabled = true;
        banner.transform.GetChild(0).Find("Text(E)").gameObject.SetActive(true);
        banner.SetActive(true);
        active = true;
        StartCoroutine(tools.Open(banner));
    }
    protected virtual void CloseInteraction()
    {
        banner.GetComponent<SpriteRenderer>().enabled = false;
        banner.transform.GetChild(0).Find("Text(E)").gameObject.SetActive(false);
    }


    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            
            if (active) // Check if interaction is active before proceeding
            {
                active = false;
                print("Exit");
                tools.ResetCamera();
                CloseAll();
            }
        }
    }

    protected virtual void CloseAll()
    {
        if (!closingInProgress)
        {
            closingInProgress = true; // A flag to indicate the closing process is in progress
            tools.AddToQueue(tools.Close(scroll));
            tools.AddToQueue(tools.Close(banner));
            tools.AddToQueue(Function(() => closingInProgress = false)); // Reset flag after all actions
            StartCoroutine(tools.ProcessCodeQueue());
        }
    }

    private IEnumerator Function(Action onCompletion = null)
    {
        CloseInteraction();
        yield return new WaitForEndOfFrame();
        onCompletion?.Invoke(); // Invoke the completion callback, if provided
        active = false;
    }
}
