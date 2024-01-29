using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class SCR_M_GIVER_A_CLASS : MonoBehaviour
{
    protected TMP_Text title;
    protected TMP_Text description;
    protected SCR_Player_Stats playerStats;
    protected GameObject player;
    public GameObject CurrentNPC;
    [SerializeField] public SCR_Tools tools;
    protected string TAG;
    

    protected virtual void StartInteraction()
    {
        tools = FindObjectOfType<SCR_Tools>();
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        title = transform.parent.GetChild(0).GetComponent<TMP_Text>();
        description = transform.parent.GetChild(1).GetComponent<TMP_Text>();
        Selection();
    }

    protected virtual void Selection() { }

    protected virtual void Update() 
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag(TAG);
        Selection();
        foreach (GameObject NPC in NPCs)
        {
            TMP_Text npcTitle = NPC.transform.GetChild(0).GetChild(0).GetChild(1).Find("Title").GetComponent<TMP_Text>();
            TMP_Text npcDescription = NPC.transform.GetChild(0).GetChild(0).GetChild(1).Find("Description").GetComponent<TMP_Text>();

            try 
            {
                TMP_Text npcReward = NPC.transform.GetChild(0).GetChild(0).GetChild(1).Find("TradeReward").GetComponent<TMP_Text>();
            }
            catch
            {
                // Do nothing
            }
            SetText(npcTitle, npcDescription);
        }
    }
    protected virtual void SetText(TMP_Text Title, TMP_Text Description) { }

    public virtual void HandleDenyReRoll(Button button)
    {
        Selection();
        CurrentNPC = button.gameObject.transform.parent.parent.parent.parent.gameObject;
        tools.ResetCamera();
        tools.AddToQueue(tools.Close(CurrentNPC.transform.GetChild(0).GetChild(0).GetChild(1).gameObject));
        tools.AddToQueue(tools.Close(CurrentNPC.transform.GetChild(0).gameObject));
        StartCoroutine(tools.ProcessCodeQueue());
    }

    public virtual void HandleAccept(Button button) { }
}
