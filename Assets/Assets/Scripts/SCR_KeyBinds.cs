using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_KeyBinds : MonoBehaviour
{
    [SerializeField] Transform menuPanel;
    Event keyEvent;
    TextMeshProUGUI buttonText;
    [SerializeField] KeyCode newKey;

    bool waitingForKey;

    // Start is called before the first frame update
    void Start()
    {
        //SCR_M_InputManager.InputManager.AssignDefaultKeys();
        menuPanel = transform.Find("Grid");
        waitingForKey = false;

        for (int i = 0; i < menuPanel.childCount; i++)
        {
            if (menuPanel.GetChild(i).name == "SetKey_UP")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_UP.ToString();
            else if (menuPanel.GetChild(i).name == "SetKey_DOWN")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_DOWN.ToString();
            else if (menuPanel.GetChild(i).name == "SetKey_LEFT")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_LEFT.ToString();
            else if (menuPanel.GetChild(i).name == "SetKey_RIGHT")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_RIGHT.ToString();
            else if (menuPanel.GetChild(i).name == "SetKey_BUTTON1")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_BUTTON1.ToString();
            else if (menuPanel.GetChild(i).name == "SetKey_BUTTON2")
                menuPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = SCR_M_InputManager.InputManager.INPUT_BUTTON2.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ...
    }

    void OnGUI()
    {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }
    public void apply() 
    {
        SCR_M_InputManager.InputManager.ApplyKeys();


    }


    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(TextMeshProUGUI text)
    {
        buttonText = text;
        Debug.Log(buttonText.text);
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey();

        // Check if the new key is already assigned to another action
        if (SCR_M_InputManager.InputManager.IsKeyAssigned(newKey, keyName))
        {
            Debug.Log("Key already assigned. Choose a different key.");
            waitingForKey = false;
            yield break;
        }

        switch (keyName)
        {
            case "up":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_UP);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_UP_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "up");
                break;
            case "down":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_DOWN);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_DOWN_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "down");
                break;
            case "left":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_LEFT);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_LEFT_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "left");
                break;
            case "right":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_RIGHT);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_RIGHT_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "right");
                break;
            case "button1":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_BUTTON1);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_BUTTON1_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "button1");
                break;
            case "button2":
                SCR_M_InputManager.InputManager.UnassignKey(SCR_M_InputManager.InputManager.INPUT_BUTTON2);
                buttonText.text = newKey.ToString();
                PlayerPrefs.SetString("INPUT_BUTTON2_KEY", newKey.ToString());
                SCR_M_InputManager.InputManager.AssignKey(newKey, "button2");
                break;
        }

        waitingForKey = false;
    }
}
