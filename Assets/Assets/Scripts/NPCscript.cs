using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCscript : MonoBehaviour
{
    private bool active;
    private GameObject Banner;
    private GameObject Scroll;
    [SerializeField] public SCR_Tools tools;
    [SerializeField] private Button AcceptButton;
    [SerializeField] private Button DenyButton;
    [SerializeField] private QuestGiver QuestGiver;
    // Start is called before the first frame update
    void Start()
    {
        QuestGiver = FindObjectOfType<QuestGiver>();
        tools = FindObjectOfType<SCR_Tools>();
        active = false;
        Banner = transform.GetChild(0).gameObject;
        Scroll = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        AcceptButton = Scroll.transform.GetChild(2).transform.GetComponent<Button>();
        DenyButton = Scroll.transform.GetChild(3).transform.GetComponent<Button>();
        DenyButton.onClick.AddListener(delegate { QuestGiver.DenyReRoll(DenyButton); });
        AcceptButton.onClick.AddListener(delegate { QuestGiver.AcceptQuest(AcceptButton); });
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON1))
        {
            //Banner.SetActive(false);
            QuestPreview();
            transform.gameObject.GetComponent<Animator>().SetBool("QuestComplete", false);
        }

    }
    void QuestPreview() 
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Scroll.SetActive(true);
        
        transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
        
        Camera.main.orthographicSize = 2;
        Camera.main.GetComponent<CameraScript>().PlayerLocked = false;
        Camera.main.transform.position = Banner.transform.position + new Vector3(-0.5f,0.5f,-15f);
        StartCoroutine(tools.Open(Scroll));
    }

    void OnTriggerEnter2D(Collider2D Collider) 
    {
        if (Collider.gameObject.tag == "Player" && Collider.gameObject.GetComponent<SCR_Player_MasterController>().quest.IsActive == false) 
        {
            Banner.SetActive(true);
            active = true;
            StartCoroutine(tools.Open(Banner));
            Collider.gameObject.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);

        }
    }
    void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            StartCoroutine(tools.Close(Scroll));
            StartCoroutine(tools.Close(Banner));
            
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            tools.ResetCamera();
            active = false;

        }
    }

    public void CloseBannar() 
    {
        StartCoroutine(tools.Close(Scroll));

    }


}
