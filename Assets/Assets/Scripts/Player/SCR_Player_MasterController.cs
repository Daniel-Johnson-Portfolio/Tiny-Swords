using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class SCR_Player_MasterController : MonoBehaviour
{

    [Header("Player Settings")]
    [SerializeField] private float PlayerMovementSpeed;
    [SerializeField] private int MaxHealth;
    [SerializeField] private int PlayerLevel;
    [SerializeField] private float CurrentXP;
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private bool CanMove;
    [SerializeField] public bool IsAlive;

    [Header("Objects")]
    [SerializeField] private TMP_Text HealthDisplay;
    [SerializeField] private Animator Animator;
    [SerializeField] private Rigidbody2D PlayerRigidbody;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private SCR_Tools tools;
    [SerializeField] private GameObject Menu;

    [Header("Conditions")]
    [SerializeField] private bool InventoryOpen;

    [Header("Quest")]
    [SerializeField] public Quest quest;

    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                UpdateHealthDisplay();
            }
        }
    }

    void Start()
    {
        if (playerConfig != null)
        {
            PlayerMovementSpeed = playerConfig.movementSpeed;
            MaxHealth = playerConfig.maxHealth;
        }

        Menu = transform.GetChild(0).Find("Menu").gameObject;
        Button button = Menu.transform.Find("Exit").GetComponent<Button>();
        button.onClick.AddListener(() => tools?.ReturnToMain());
        tools = FindObjectOfType<SCR_Tools>();
        PlayerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        
        HealthDisplay = gameObject.transform.Find("Canvas/Health/Health/Text (TMP)").GetComponent<TMP_Text>();
        CurrentHealth = MaxHealth;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        parentTransform = transform.parent;
        CanMove = true;
        IsAlive = true;

        StartCoroutine(tools.FadeOut(transform.GetChild(0).Find("Black").GetComponent<Image>()));
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleInventoryInput();
            HandleMenuInput();
            RotatePlayer();
        }
        if (!IsAlive && Input.GetKey(SCR_M_InputManager.InputManager.INPUT_RESPAWN)) 
        {
            Respawn();
        }
        
        if (currentHealth <= 0 && IsAlive) 
        {
            IsAlive = false;
            Dead();
        }
    }
    private void Dead() 
    {
        Animator.SetBool("IsDead", true);
        Animator.SetTrigger("Dead");
        CanMove = false;
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        tools.AddToQueue(tools.FadeIn(transform.GetChild(0).Find("Black").GetComponent<Image>()));
        tools.AddToQueue(tools.FadeIn(transform.GetChild(0).Find("RespawnUI").GetChild(0).GetComponent<Image>()));
        tools.AddToQueue(tools.FadeIn(transform.GetChild(0).Find("RespawnUI").GetChild(1).GetComponent<Image>()));
        tools.StartCoroutine(tools.ProcessCodeQueue());
     
    }
    private void Respawn()
    {
        Animator.SetBool("IsDead", false);
        
        CanMove = true;
        PlayerRigidbody.constraints = RigidbodyConstraints2D.None;
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        tools.AddToQueue(tools.FadeOut(transform.GetChild(0).Find("Black").GetComponent<Image>()));
        tools.AddToQueue(tools.FadeOut(transform.GetChild(0).Find("RespawnUI").GetChild(0).GetComponent<Image>()));
        tools.AddToQueue(tools.FadeOut(transform.GetChild(0).Find("RespawnUI").GetChild(1).GetComponent<Image>()));
        tools.StartCoroutine(tools.ProcessCodeQueue());
        IsAlive = true;
        CurrentHealth = MaxHealth;
        //Reset character sprite
        //Set player position to last checkpoint
    }

    private void UpdateHealthDisplay()
    {
        if (HealthDisplay != null)
        {
            HealthDisplay.text = (currentHealth / 10).ToString();
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = PlayerRigidbody.position + movementInput * PlayerMovementSpeed * Time.fixedDeltaTime;
        PlayerRigidbody.MovePosition(newPosition);
        UpdateAnimationSpeed();
    }

    private void UpdateAnimationSpeed()
    {
        float speed = movementInput.normalized.magnitude * PlayerMovementSpeed;
        Animator.SetFloat("Speed", speed);
    }

    private void HandleMovementInput() 
    {
        movementInput = Vector2.zero;
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_UP))
        {
            movementInput.y += 1;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_DOWN))
        {
            movementInput.y -= 1;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_LEFT))
        {
            movementInput.x -= 1;
        }
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_RIGHT))
        {
            movementInput.x += 1;
        }
      
    }
    private void HandleInventoryInput() 
    {
        if (Input.GetKeyDown(SCR_M_InputManager.InputManager.INPUT_INVENTORY))
        {

            if (!InventoryOpen)
            {
                StartCoroutine(tools.Open(gameObject.transform.GetChild(0).GetChild(2).gameObject));
                InventoryOpen = true;
            }
            else
            {
                StartCoroutine(tools.Close(gameObject.transform.GetChild(0).GetChild(2).gameObject));
                InventoryOpen = false;
            }
        }

    }

    private void HandleMenuInput() 
    {
        if (Input.GetKey(SCR_M_InputManager.InputManager.INPUT_MENU))
        {
            Menu.SetActive(true);
        }
    }

    private void RotatePlayer()
    {
        if (movementInput.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (movementInput.x > 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public Vector3 GetPlayerMovementDirection()
    {
        return movementInput;
    }


}

