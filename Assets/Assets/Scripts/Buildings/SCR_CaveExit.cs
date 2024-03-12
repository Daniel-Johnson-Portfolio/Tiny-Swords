using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaveExit : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            Activate();
        }
    }

    public void Activate()
    {
        tools.AddToQueue(tools.Close(Banner));
        tools.AddToQueue(tools.FadeIn(Player.transform.GetChild(0).GetChild(4).GetComponent<Image>()));
        tools.AddToQueue(Function());
        tools.AddToQueue(tools.FadeOut(Player.transform.GetChild(0).GetChild(4).GetComponent<Image>()));
        StartCoroutine(tools.ProcessCodeQueue());
    }

    public IEnumerator Function()
    {
        float x = PlayerPrefs.GetFloat("CaveX");
        float y = PlayerPrefs.GetFloat("CaveY");

        Player.transform.position = new Vector3(x, y, 0);
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            active = true;
            Player = Collider.gameObject;
            StartCoroutine(tools.Open(Banner));
        }
    }

    void OnTriggerExit2D(Collider2D Collider)
    {
         if (Collider.gameObject.tag == "Player")
        {
            active = false;

            if (Player != null)
            {
                Image playerImage = Player.transform.GetChild(0).GetChild(4).GetComponent<Image>();

                if (playerImage != null)
                {
                    StartCoroutine(tools.FadeOut(playerImage));
                }
                else
                {
                    Debug.LogWarning("Player image component is null.");
                }
            }
            else
            {
                Debug.LogWarning("Player object is null.");
            }

            StartCoroutine(tools.Close(Banner));
        }
    }

}