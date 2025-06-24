using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanelV1;
    public TMP_Text tutorialTextV1;
    
    //iconinput
    public Image defautIcon;
    public Sprite walkIcon;
    public Sprite runIcon;
    public Sprite jumpIcon;
    public Sprite rollBackIcon;
    public Sprite attackIcon;
    public Sprite interactNPC;//tuong tac huong dan

    private int currentStep = 0;
    private bool[] stepCompleted;

    //tutorial hud
    public GameObject turorialRun;
    public GameObject tutorialEnemy;
    public GameObject pointer1;
    public GameObject tutorialInteractNPC;
    public float enemy; //huoung dan danh quai

    //bion để kiểm soát trạng thái của hướng dẫn
    public bool isTutorialWalk = false;
    public bool isTutorialRun = false; // Biến để kiểm soát trạng thái của hướng dẫn chạy
    public bool isComplete = false; // Biến để kiểm soát trạng thái hoàn thành quest ne quai
    //tham chieu
    private SlowMotionDodgeEvent slowMotion;
    private PlayerControllerState playerControllerState;
    private AnimatorPanelTutorial animatorPanelTutorial;
    private InteractNPC interactNpc;
    void Start()
    {
        slowMotion = FindAnyObjectByType<SlowMotionDodgeEvent>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        animatorPanelTutorial = FindAnyObjectByType<AnimatorPanelTutorial>();
        interactNpc = FindAnyObjectByType<InteractNPC>();

        stepCompleted = new bool[10];
        StartCoroutine(WaitShowStep(0));
        tutorialPanelV1.SetActive(false);
        playerControllerState.isRun = false;
        playerControllerState.isJump = false; // Đặt trạng thái lăn là false ban đầu
        playerControllerState.isRollBack = false; // Đặt trạng thái lăn về sau là false ban đầu
        playerControllerState.isAttack = false; // Đặt trạng thái tấn công là false ban đầu
        turorialRun.SetActive(false);
        tutorialEnemy.SetActive(false);
        pointer1.SetActive(false);
        tutorialInteractNPC.SetActive(false);
    }

    void Update()
    {
        switch (currentStep)
        {
            case 0://huong dan di chuyen a,d,s,w 
                if (isTutorialWalk)            
                {                 
                    stepCompleted[0] = true;
                    StartCoroutine(WaitShowStep(1)); // Hiển thị bước tiếp theo sau 1 giây
                }
                break;
            case 1://huong dan chay nhanh shift
                if (isTutorialRun)
                {        
                    stepCompleted[1] = true;
                    StartCoroutine(WaitShowStep(2));
                }
                break;
            case 2://huong dan nhay 
                if (Input.GetKeyDown(KeyCode.Space))
                {
                               
                    stepCompleted[2] = true;
                    StartCoroutine(WaitShowStep(3));
                }
                break;

            case 3://huong dan lan ve phia sau
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    {                     
                        stepCompleted[3] = true;
                        tutorialEnemy.SetActive(true);
                        StartCoroutine(WaitShowStep(4)); // Hiển thị bước tiếp theo sau 1 giây
                     
                    }
                    
                }
                break;
            case 4://huong dan né 
                if (slowMotion.isDodgeWindowActive && !tutorialPanelV1.activeSelf)
                {
                    tutorialPanelV1.SetActive(true);                 
                    defautIcon.sprite = rollBackIcon;
                  
                }
                if (Input.GetKeyDown(KeyCode.LeftControl) && slowMotion.isDodgeWindowActive)
                {
                    slowMotion.ResetTime();
                    stepCompleted[4] = true;
                    StartCoroutine(WaitShowStep(5));
                }

                // Nếu nguoi choi không né trong khoảng thời gian nhất định thi hoàn thành luôn :))
                if (!slowMotion.isDodgeWindowActive && isComplete)
                {
                    slowMotion.ResetTime();
                    stepCompleted[4] = true;
                    playerControllerState.isAttack = true;
                    StartCoroutine(WaitShowStep(5));
                }
                break;

            case 5://huong dan danh enemy
                if (enemy >= 1)
                {
                    tutorialInteractNPC.SetActive(true);//bat box cho npc len
                    stepCompleted[5] = true;
                    pointer1.SetActive(true);
                    StartCoroutine(WaitShowStep(6));
                }
                break;
            case 6:
               
                break;
            case 7://tuong tac npc nhan nvu
                if (interactNpc.isInteract == true)
                {
                    tutorialPanelV1.SetActive(true);

                }
                if (interactNpc.isInteract == true && Input.GetKeyDown(KeyCode.LeftControl))
                {


                }
                break;
        }
    }



    public IEnumerator WaitShowStep(int step)
    {
     
        tutorialPanelV1.SetActive(false); // Ẩn panel trước khi hiển thị bước mới
        yield return new WaitForSeconds(1f);    
        tutorialPanelV1.SetActive(true); 
        currentStep = step;

        switch (step)
        {
            case 0:
                tutorialTextV1.text = "Nhấn W/A/S/D để di chuyển.";
                defautIcon.sprite = walkIcon;
                break;

            case 1:
                tutorialTextV1.text = "Nhấn Shift để chạy nhanh.";
                defautIcon.sprite = runIcon;
                //hoan thanh thi dc 
                playerControllerState.isRun = true;
                turorialRun.SetActive(true);
                break;

            case 2:
                tutorialTextV1.text = "Nhấn Space để nhảy.";
                defautIcon.sprite = jumpIcon;
                playerControllerState.isJump = true; // Đặt trạng thái jump là true
                break;

            case 3:
                tutorialTextV1.text = "Nhấn Ctrl để lăn về phía sau.";
                defautIcon.sprite = rollBackIcon;
                playerControllerState.isRollBack = true; // Đặt trạng thái rollback là true
                break;
            case 4:              
                tutorialTextV1.text = "Nhấn Ctrl để né!";
                defautIcon.sprite = rollBackIcon;
                tutorialPanelV1.SetActive(false); // Ẩn panel trước khi hiển thị bước mới
                break;
            case 5:
                tutorialTextV1.text = "Nhấn chuột trái để tấn công.";
                defautIcon.sprite = attackIcon;
                playerControllerState.isAttack = true;
               
                break;
            case 6:
                tutorialTextV1.text = "Chạm vào vật phẩm để nhặt.";
                defautIcon.enabled = false;
                tutorialPanelV1.SetActive(false); // Ẩn panel trước khi hiển thị bước mới
                break;
            case 7:
                tutorialTextV1.text = "Nhấn F để tương tác.";
                defautIcon.sprite = interactNPC;
                tutorialPanelV1.SetActive(false); // Ẩn panel trước khi hiển thị bước mới
                break;
        }
    }
    
    public void UpdateEnemyTutorial(float amount)
    {
        enemy += amount;

    }
}
