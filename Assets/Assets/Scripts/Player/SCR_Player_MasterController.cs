using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_Player_MasterController : MonoBehaviour
{

    [SerializeField] private float PlayerMovementSpeed = 2f;
    [SerializeField] private int MaxHealth = 1000;
    [SerializeField] private TMP_Text HealthDisplay;
    [SerializeField] private bool InventoryOpen;
    [SerializeField] private int PlayerLevel;
    [SerializeField] private float CurrentXP;

    private Animator Animator;

    private Rigidbody2D PlayerRigidbody;
    private Transform parentTransform;
    public Vector3 PlayerMovmentDirection;
    public Quest quest;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }
    private int currentHealth;



    // Start is called before the first frame update
    void Start()
    {
        PlayerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        StartCoroutine(FadeIn(transform.GetChild(0).GetChild(4).GetComponent<Image>()));
        HealthDisplay = transform.GetChild(0).GetChild(3).GetChild(4).GetChild(0).GetComponent<TMP_Text>();
        CurrentHealth = MaxHealth;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        parentTransform = transform.parent;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lastPos = transform.position;
        HealthDisplay.text = (CurrentHealth / 10).ToString();

        InputHandling();
       
        PlayerMovmentDirection = transform.position - lastPos;

        float speed = Vector3.Distance(transform.position, lastPos) / Time.deltaTime; //Speed = Distance/time
        if (speed > 0)
        {
            Animator.SetFloat("Speed", speed);
        }
        else
        {
            Animator.SetFloat("Speed", 0);
        }

        RotatePlayer();
    }

    void InputHandling() 
    {
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_UP))
        {
            transform.position += (Vector3.up * Time.deltaTime) * PlayerMovementSpeed;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_DOWN))
        {
            transform.position += (Vector3.down * Time.deltaTime) * PlayerMovementSpeed;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_LEFT))
        {
            transform.position += (Vector3.left * Time.deltaTime) * PlayerMovementSpeed;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_RIGHT))
        {
            transform.position += (Vector3.right * Time.deltaTime) * PlayerMovementSpeed;
        }
        else
        {
            PlayerMovmentDirection = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_INVENTORY))
        {

            if (!InventoryOpen)
            {
                StartCoroutine(Open(gameObject.transform.GetChild(0).GetChild(2).gameObject));
                InventoryOpen = true;
            }
            else
            {
                StartCoroutine(Close(gameObject.transform.GetChild(0).GetChild(2).gameObject));
                InventoryOpen = false;
            }
        }

    }

    private void RotatePlayer()
    {
        if (PlayerMovmentDirection.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (PlayerMovmentDirection.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void FixedUpdate()
    {
        //PlayerRigidbody.velocity = PlayerMovmentDirection * PlayerMovementSpeed;
    }

    private IEnumerator Open(GameObject ToOpen)
    {
        while (ToOpen.transform.localScale != new Vector3(1f, 1f, 1f))
        {
            ToOpen.transform.localScale = ToOpen.transform.localScale + new Vector3(0f, 0.1f, 0f);
            yield return new WaitForSeconds(0.001f);
        }
    }
    private IEnumerator Close(GameObject ToClose)
    {
        while (ToClose.transform.localScale.y > 0)
        {
            ToClose.transform.localScale += new Vector3(0f, -Mathf.Min(0.1f, ToClose.transform.localScale.y), 0f);
            yield return new WaitForSeconds(0.01f);

        }
    }
    private IEnumerator FadeIn(Image ToFade)
    {
        while (ToFade.color != new Color(0, 0, 0, 0))
        {
            ToFade.color = ToFade.color + new Color(0, 0, 0, -Mathf.Min(0.01f, ToFade.color.a));
            yield return new WaitForSeconds(0.01f);
        }
        transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
    }

    public Vector3 GetPlayerMovementDirection()
    {
        return PlayerMovmentDirection;
    }


}

