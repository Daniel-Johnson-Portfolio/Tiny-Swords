using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveQuest : MonoBehaviour
{
    public Quest CurrentQuest;
    private TMP_Text Title;
    private TMP_Text Description;
    public QuestGoal questGoal;
    public int RequiredAmount;
    public string GatheringType;
    private bool isCoroutineRunning = false;
    public GameObject CurrentNPC;
    protected SCR_Player_Stats playerStats;
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    [SerializeField] protected int LogsStartingAmount;
    [SerializeField] protected int KillStartingAmount;

    [System.Serializable]
    public struct Inventory
    {
        public int wood;
        public int gold;
        public int meat;
    }
    private Inventory PlayerInventory;

    void Start()
    {
        playerStats = FindObjectOfType<SCR_Player_Stats>();
        Title = gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        Description = gameObject.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
    }

    void Update()
    {
        CurrentNPC = FindAnyObjectByType<QuestGiver>().GetComponent<SCR_QUEST_GIVER>().CurrentNPC;
        CurrentQuest = gameObject.GetComponent<SCR_Player_MasterController>().quest;
        Title.text = CurrentQuest.Title;
        Description.text = CurrentQuest.Description;
        questGoal = gameObject.GetComponent<SCR_Player_MasterController>().quest.goal;
        RequiredAmount = questGoal.requiredAmount;
        InventoryUpdate();

        if (CurrentQuest.IsActive && questGoal.Goaltype == Goaltype.Gathering)
        {
            GatheringType = CurrentQuest.TypeToGather;
            GatheringChecker();
        }
        if (CurrentQuest.IsActive && questGoal.Goaltype == Goaltype.kill)
        {
            KillChecker();
        }
    }

    void InventoryUpdate()
    {
        PlayerInventory.wood = TryParseToInt(gameObject.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        PlayerInventory.gold = TryParseToInt(gameObject.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text);
        PlayerInventory.meat = TryParseToInt(gameObject.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text);
    }

    int TryParseToInt(string text)
    {
        int result = 0;
        int.TryParse(text, out result);
        return result;
    }

    void GatheringChecker()
    {
        if (GatheringType == "Wood")
        {
            if (questGoal.CurrentAmount == 0 && LogsStartingAmount == 0)
            {
                LogsStartingAmount = playerStats.resourceStats.WoodCollected;
            }
            if (LogsStartingAmount < playerStats.resourceStats.WoodCollected)
            {
                questGoal.CurrentAmount++;
                LogsStartingAmount = playerStats.resourceStats.WoodCollected;
            }
            if (questGoal.CurrentAmount == questGoal.requiredAmount)
            {
                LogsStartingAmount = playerStats.resourceStats.WoodCollected;
                Complete();
            }
        }
    }
    void KillChecker()
    {
        
            if (questGoal.CurrentAmount == 0 && KillStartingAmount == 0)
            {
                KillStartingAmount = playerStats.goblinStats.AmountKilled;
            }
            if (KillStartingAmount < playerStats.goblinStats.AmountKilled)
            {
                questGoal.CurrentAmount++;
                KillStartingAmount = playerStats.goblinStats.AmountKilled;
            }
            if (questGoal.CurrentAmount == questGoal.requiredAmount)
            {
                KillStartingAmount = playerStats.goblinStats.AmountKilled;
                Complete();
            }
        
    }



    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "QuestLocation" && CurrentQuest.IsActive && questGoal.Goaltype == Goaltype.Location)
        {
            CurrentQuest.goal.LocationReached();

            if (CurrentQuest.goal.IsReached())
            {
                Complete();
            }
        }
    }

    void Complete()
    {
        CurrentNPC.GetComponent<Animator>().SetBool("QuestComplete", true);

        playerStats.IncrementXP(CurrentQuest.XPReward);
        questGoal.CurrentAmount = 0;
        gameObject.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(true);
        EnqueueCoroutine(FadeIn(gameObject.transform.GetChild(0).GetChild(1).GetChild(2).gameObject));
        EnqueueCoroutine(FadeOut(gameObject.transform.GetChild(0).GetChild(1).GetChild(0).gameObject));
        EnqueueCoroutine(FadeOut(gameObject.transform.GetChild(0).GetChild(1).GetChild(1).gameObject));
        EnqueueCoroutine(Close(gameObject.transform.GetChild(0).GetChild(1).gameObject));
        EnqueueCoroutine(Close(gameObject.transform.GetChild(0).GetChild(0).gameObject));

        StartCoroutine(ProcessCoroutineQueue());

        CurrentQuest.Complete();
    }

    private void EnqueueCoroutine(IEnumerator coroutine)
    {
        coroutineQueue.Enqueue(coroutine);
    }

    private IEnumerator ProcessCoroutineQueue()
    {
        if (isCoroutineRunning)
            yield break;

        isCoroutineRunning = true;

        while (coroutineQueue.Count > 0)
        {
            IEnumerator nextCoroutine = coroutineQueue.Dequeue();
            yield return StartCoroutine(nextCoroutine);
            yield return new WaitForSeconds(0.5f);
        }

        isCoroutineRunning = false;
    }

    private IEnumerator FadeIn(GameObject toFadeIn)
    {
        Toggle toggle = toFadeIn.GetComponent<Toggle>();
        while (toggle.colors.disabledColor.a > 0)
        {
            ColorBlock colorBlock = toggle.colors;
            Color newColor = colorBlock.disabledColor - new Color(0, 0, 0, Mathf.Clamp(0.1f, 0f, 255f) * colorBlock.disabledColor.a / 255f);
            colorBlock.disabledColor = newColor;
            toggle.colors = colorBlock;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FadeOut(GameObject toClose)
    {
        while (toClose.GetComponent<TextMeshPro>().color.a > 0)
        {
            toClose.GetComponent<TextMeshPro>().color += new Color(0, 0, 0, -Mathf.Min(0.1f, toClose.GetComponent<TextMeshPro>().color.a));
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator Close(GameObject toClose)
    {
        while (toClose.transform.localScale.y > 0)
        {
            toClose.transform.localScale += new Vector3(0, -Mathf.Min(0.1f, toClose.transform.localScale.y), 0f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
