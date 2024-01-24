using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class NPCscript : MonoBehaviour
{
    private bool active;
    private GameObject Banner;
    private GameObject Scroll;
    [SerializeField] public SCR_Tools tools;
    // Start is called before the first frame update
    void Start()
    {
        tools = FindObjectOfType<SCR_Tools>();
        active = false;
        Banner = transform.GetChild(0).gameObject;
        Scroll = transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true && SCR_M_InputManager.InputManager.InputRequest(KeyCode.E))
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
        Camera myCamera = Camera.main;
        myCamera.orthographicSize = 2;
        myCamera.GetComponent<CameraScript>().PlayerLocked = false;
        myCamera.transform.position = Banner.transform.position + new Vector3(-0.5f,0.5f,-15f);
        StartCoroutine(tools.Open(Scroll));
    }
    //.GetComponent<TextMeshPro>()

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
            Camera.main.GetComponent<CameraScript>().PlayerLocked = true;
            Camera.main.orthographicSize = 5;
            active = false;

        }
    }

    public void CloseBannar() 
    {
        StartCoroutine(tools.Close(Scroll));

    }


}
