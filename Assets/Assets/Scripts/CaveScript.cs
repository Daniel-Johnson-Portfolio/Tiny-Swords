using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class CaveScript : MonoBehaviour
{
    [SerializeField] private GameObject Banner;
    [SerializeField] private bool active;
    [SerializeField] private GameObject Player;
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] public SCR_Tools tools;

    void Start()
    {
        tools = FindObjectOfType<SCR_Tools>();
        Banner = gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            Activate();
        }
    }

    public void Activate()
    {
        StartCoroutine(tools.Close(Banner));
        PlayerPrefs.SetFloat("CaveX", Player.transform.position.x);
        PlayerPrefs.SetFloat("CaveY", Player.transform.position.y);

        tools.AddToQueue(tools.FadeIn(Player.transform.GetChild(0).GetChild(4).GetComponent<Image>()));
        tools.AddToQueue(Function());
        tools.AddToQueue(tools.FadeOut(Player.transform.GetChild(0).GetChild(4).GetComponent<Image>()));
        StartCoroutine(tools.ProcessCodeQueue());
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
    public IEnumerator Function()
    {
        Player.transform.position = new Vector3(-249, -2, 0);
        SceneManager.LoadSceneAsync("Cave", LoadSceneMode.Additive);
        yield return null;
    }
}