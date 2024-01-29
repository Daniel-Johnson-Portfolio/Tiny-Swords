using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NPC_CLASS : MonoBehaviour
{
    [SerializeField] protected SCR_Tools tools;
    [SerializeField] private SCR_QUEST_GIVER questGiver;
    [SerializeField] protected SCR_TradeGiver TradeGiver;

    private bool active;
    [SerializeField] private GameObject banner;
    private GameObject scroll;

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
        denyButton = scroll.transform.Find("Deny").GetComponent<Button>();  //MayCauseNullReferenceException
        SetButtons();
    }

    protected virtual void HandleInteractionInput()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON1))
        {
            QuestPreview();
            transform.GetComponent<Animator>().SetBool("QuestComplete", false);
        }
    }

    protected virtual void QuestPreview()
    {
        banner.GetComponent<SpriteRenderer>().enabled = false;
        scroll.SetActive(true);

        banner.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        Camera.main.orthographicSize = 2;
        Camera.main.GetComponent<CameraScript>().PlayerLocked = false;
        Camera.main.transform.position = banner.transform.position + new Vector3(-0.5f, 0.5f, -15f);
        StartCoroutine(tools.Open(scroll));
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.GetComponent<SCR_Player_MasterController>().quest.IsActive == false)
        {
            banner.SetActive(true);
            active = true;
            StartCoroutine(tools.Open(banner));
            collider.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            StartCoroutine(tools.Close(scroll));
            StartCoroutine(tools.Close(banner));

            banner.GetComponent<SpriteRenderer>().enabled = true;
            banner.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
            banner.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            tools.ResetCamera();
            active = false;
        }
    }

    protected virtual void CloseAll()
    {
        tools.AddToQueue(tools.Close(scroll));
        tools.AddToQueue(tools.Close(banner));
        StartCoroutine(tools.ProcessCodeQueue());
    }
}
