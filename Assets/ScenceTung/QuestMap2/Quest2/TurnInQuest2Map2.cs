using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TurnInQuest2Map2 : MonoBehaviour
{
   NavMeshAgent agent;
    Animator anmt;
    public GameObject canvasText;
    public TMP_Text contentText;
    public bool isQuest2 = false;
   public bool isDone = false;
    public GameObject canvasContent;
    public GameObject btnF;
    public GameObject turnInQuest;
    public Transform targetPos; // <-- Vị trí muốn NPC đi tới
    
    private bool isMoving = false;
    private bool isArrived = false;
    public GameObject canvasQuest;
    public GameObject activeQuest;
    public GameObject quest2;
    AudioSource audioSource;
    public AudioClip clip1;
    public bool eventQuestBool = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        anmt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone && !isMoving)
        {
            canvasContent.SetActive(true);
            btnF.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && btnF.activeSelf)
            {
                btnF.SetActive(false);
                StartCoroutine(DoneQuest());
                turnInQuest.SetActive(false);
            }
        }

        if (isMoving && !isArrived)
        {
            float distance = Vector3.Distance(transform.position, targetPos.position);
            float eventQuest = Vector3.Distance(transform.position, targetPos.position);
            if (distance < 1f)
            {
                isArrived = true;
                agent.ResetPath();
                quest2.SetActive(false);
                gameObject.SetActive(false); // Ẩn NPC sau khi đến nơi
            }
            if(eventQuest < 80f && !eventQuestBool)
            {
              StartCoroutine(corotine());
            }
        }
    }
    IEnumerator DoneQuest()
    {
        
      
        canvasText.SetActive(true);
        contentText.text = "Tôi đến để gửi tin cho lính gác ở đội 7.";
        isQuest2 = true;
        activeQuest.SetActive(true);
        yield return new WaitForSeconds(2f);

        canvasContent.SetActive(false);
        canvasText.SetActive(false);
        canvasQuest.SetActive(false);
        

        // Bắt đầu di chuyển tới vị trí target
        isMoving = true;
        anmt.SetTrigger("Run");
        agent.SetDestination(targetPos.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isDone = true;
        }
    }

    IEnumerator corotine()
    {
        eventQuestBool = true;
        audioSource.clip = clip1;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        canvasText.SetActive(true);
        contentText.text = " Có tiếng gì vậy?? ";
        yield return new WaitForSeconds(2f);
        canvasText.SetActive(false);

    }
}
