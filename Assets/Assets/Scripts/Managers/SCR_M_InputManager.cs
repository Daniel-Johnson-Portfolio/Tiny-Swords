using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_M_InputManager : MonoBehaviour
{
    public static SCR_M_InputManager InputManager;

    public KeyCode INPUT_UP { get; set; }
    public KeyCode INPUT_DOWN { get; set; }
    public KeyCode INPUT_LEFT { get; set; }
    public KeyCode INPUT_RIGHT { get; set; }
    public KeyCode INPUT_ATTACK { get; set; }
    public KeyCode INPUT_BUTTON1 { get; set; }
    public KeyCode INPUT_BUTTON2 { get; set; }
    public KeyCode INPUT_INVENTORY { get; set; }
    public KeyCode INPUT_MENU { get; set; }

    private Dictionary<KeyCode, string> assignedKeys = new Dictionary<KeyCode, string>();

    private void Awake()
    {
        if (InputManager == null)
        {
            DontDestroyOnLoad(gameObject);
            InputManager = this;
        }
        else if (InputManager != this)
        {
            Destroy(gameObject);
        }

        ApplyKeys();
    }

    public void AssignDefaultKeys()
    {
        assignedKeys.Clear();  // Clear the dictionary to start fresh

        // Add default key bindings to the dictionary
        assignedKeys.Add(INPUT_UP, "up");
        assignedKeys.Add(INPUT_DOWN, "down");
        assignedKeys.Add(INPUT_LEFT, "left");
        assignedKeys.Add(INPUT_RIGHT, "right");
        assignedKeys.Add(INPUT_ATTACK, "attack");
        assignedKeys.Add(INPUT_BUTTON1, "button1");
        assignedKeys.Add(INPUT_BUTTON2, "button2");
        assignedKeys.Add(INPUT_MENU, "menu");
    }



    KeyCode GetKeyCodeFromPlayerPrefs(string key, string defaultValue)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(key, defaultValue));
    }

    public bool IsKeyAssigned(KeyCode key, string currentAction)
    {
        return assignedKeys.ContainsKey(key) && assignedKeys[key] != currentAction;
    }

    public void AssignKey(KeyCode key, string action)
    {
        assignedKeys[key] = action;
    }

    public void UnassignKey(KeyCode key) 
    {
        if (assignedKeys.ContainsKey(key))
        {
            Debug.Log(key);

            assignedKeys.Remove(key);
        } 
    }
    public void ApplyKeys() 
    {
        INPUT_UP = GetKeyCodeFromPlayerPrefs("INPUT_UP_KEY", "W");
        INPUT_DOWN = GetKeyCodeFromPlayerPrefs("INPUT_DOWN_KEY", "S");
        INPUT_LEFT = GetKeyCodeFromPlayerPrefs("INPUT_LEFT_KEY", "A");
        INPUT_RIGHT = GetKeyCodeFromPlayerPrefs("INPUT_RIGHT_KEY", "D");
        INPUT_ATTACK = GetKeyCodeFromPlayerPrefs("INPUT_ATTACK_KEY", "Mouse0");
        INPUT_BUTTON1 = GetKeyCodeFromPlayerPrefs("INPUT_BUTTON1_KEY", "E");
        INPUT_BUTTON2 = GetKeyCodeFromPlayerPrefs("INPUT_BUTTON2_KEY", "F");
        INPUT_INVENTORY = GetKeyCodeFromPlayerPrefs("INPUT_INVENTORY_KEY", "I");
        INPUT_MENU = GetKeyCodeFromPlayerPrefs("INPUT_MENU_KEY", "Escape");

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool InputRequest(KeyCode requested)
    {
        if (Input.GetKey(requested))
        {
            return true;
        }
        return false;
    }
}
