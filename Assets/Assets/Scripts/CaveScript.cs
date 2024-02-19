using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaveScript : MonoBehaviour
{
    [SerializeField] private GameObject Banner;
    [SerializeField] private GameObject Player;
    [SerializeField] private Image PlayerImage; // Assume you assign this via the inspector
    private bool active;
    private SCR_Tools tools;

    private void Start()
    {
        active = false;
        tools = FindObjectOfType<SCR_Tools>();
        Banner = transform.GetChild(0).GetChild(0).gameObject;
    }

    private void Update()
    {
        if (active && Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_BUTTON2))
        {
            Activate();
        }
    }

    public void Activate()
    {
        StartCoroutine(tools.Close(Banner));
        SavePlayerPosition();
        SequencePlayerFade();
        StartCoroutine(tools.ProcessCodeQueue());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !active)
        {
            SetActiveState(true, collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            SetActiveState(false, collider.gameObject);
        }
    }

    private void SetActiveState(bool state, GameObject player)
    {
        active = state;
        Player = player;
        PlayerImage = Player.transform.GetChild(0).Find("Black").GetComponent<Image>(); // Cache this reference more efficiently

        if (state)
        {
            StartCoroutine(tools.Open(Banner));
        }
        else
        {
            StartCoroutine(tools.Close(Banner));
        }
    }

    private void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("CaveX", Player.transform.position.x);
        PlayerPrefs.SetFloat("CaveY", Player.transform.position.y);
    }

    private void SequencePlayerFade()
    {
        if (PlayerImage != null)
        {
            tools.AddToQueue(tools.FadeIn(PlayerImage));
            tools.AddToQueue(Function());
            tools.AddToQueue(tools.FadeOut(PlayerImage));
        }
    }

    public IEnumerator Function()
    {
        Player.transform.position = new Vector3(-249, -2, 0);
        SceneManager.LoadSceneAsync("Cave", LoadSceneMode.Additive);
        yield return null;
    }
}
