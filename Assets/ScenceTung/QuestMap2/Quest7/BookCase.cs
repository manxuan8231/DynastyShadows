using System.Collections;
using TMPro;
using UnityEngine;

public class BookCase : MonoBehaviour
{

    [Header("Canvas UI")]
    public GameObject canvasQuest;
    public TMP_Text questContent;

    public GameObject canvasInteraction;
    public GameObject btnF;

    public GameObject canvasQuestGame;
    public TMP_Text questGameContent;

    [Header("Book Settings")]
    public bool isHasbook = false;
    public bool isCanOpen = false;
    bool isContent = false;
    public bool isActiveBtn = false;
    public GameObject back;

    [Header("Tham chiếu")]
    public NPCDialogueController npcDialogueController;
    public AwardQuest AwardQuest;
    
    private void Start()
    {
        npcDialogueController = FindAnyObjectByType<NPCDialogueController>();
        AwardQuest = FindAnyObjectByType<AwardQuest>();
    }

    private void Update()
    {
        if(npcDialogueController.currentStage == QuestStage.Quest7Stage2)
        {

            if (isCanOpen && !isActiveBtn)
            {
                canvasInteraction.SetActive(true);
                btnF.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F) && isHasbook && !isContent)
                {
                    StartCoroutine(HasBook());
                }
                if (Input.GetKeyDown(KeyCode.F) && !isHasbook && !isContent)
                {
                    StartCoroutine(NoBook());
                }
            }
        }
       

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" )
        {
            isCanOpen = true;   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            btnF.SetActive(false);
            isCanOpen = false;
        }
    }

    IEnumerator HasBook()   
    {
        isContent = true;
        isActiveBtn = true;
        canvasInteraction.SetActive(false);
        canvasQuest.SetActive(true);
        questContent.text = "Đây rồi ! chính là cuốn sách này";
        npcDialogueController.currentStage = QuestStage.Quest7Stage3;
        npcDialogueController.HandleQuestProgression();     
        AwardQuest.AwardQuest7();
        yield return new WaitForSeconds(2f);
        canvasQuest.SetActive(false);
        canvasQuestGame.SetActive(true);
        questGameContent.text = "Mang về cho trưởng mục Lương ";
        back.SetActive(true);



    }
    IEnumerator NoBook()
    {
        isContent = true;
        canvasQuest.SetActive(true);
        canvasInteraction.SetActive(true);
        questContent.text = "Không có sách nào ở đây cả";
        yield return new WaitForSeconds(2f);
        canvasQuest.SetActive(false);
        isContent = false;
        isActiveBtn = false;
    }
}
