using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanelV1;
    public TMP_Text tutorialTextV1;
    public GameObject tutorialPanelV2;
    public TMP_Text tutorialTextV2;
    //iconinput
    public Image defautIcon;
    public Sprite walkIcon;
    public Sprite runIcon;
    public Sprite jumpIcon;
    public Sprite rollBackIcon;
    public Sprite attackIcon;

    private int currentStep = 0;
    private bool[] stepCompleted;

    //bion để kiểm soát trạng thái của hướng dẫn
    public bool isTutorialWalk = false;
    public bool isTutorialRun = false; // Biến để kiểm soát trạng thái của hướng dẫn chạy

    public bool isComplete = false; // Biến để kiểm soát trạng thái hoàn thành quest ne quai
    //tham chieu
    private SlowMotionDodgeEvent slowMotion;
    private PlayerControllerState playerControllerState;
    private AnimatorPanelTutorial animatorPanelTutorial;
    void Start()
    {
        stepCompleted = new bool[5];
        StartCoroutine(WaitShowStep(0));
        slowMotion = FindAnyObjectByType<SlowMotionDodgeEvent>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        animatorPanelTutorial = FindAnyObjectByType<AnimatorPanelTutorial>();
        playerControllerState.isRun = false;
        playerControllerState.isJump = false; // Đặt trạng thái lăn là false ban đầu
        playerControllerState.isRollBack = false; // Đặt trạng thái lăn về sau là false ban đầu
        playerControllerState.isAttack = false; // Đặt trạng thái tấn công là false ban đầu
    }

    void Update()
    {
        switch (currentStep)
        {
            case 0:
                if (isTutorialWalk)//huong dan di chuyen a,d,s,w             
                {
                    //hoan thanh thi dc 
                    playerControllerState.isRun = true;
                    stepCompleted[0] = true;

                    StartCoroutine(WaitShowStep(1)); // Hiển thị bước tiếp theo sau 1 giây
                }
                break;
            case 1:
                if (isTutorialRun)//huong dan chay nhanh shift
                {
                    playerControllerState.isJump = true; // Đặt trạng thái jump là true
                    stepCompleted[1] = true;
                    StartCoroutine(WaitShowStep(2));
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Space))//huong dan nhay ve phia trc
                {
                    playerControllerState.isRollBack = true; // Đặt trạng thái rollback là true
                    stepCompleted[2] = true;
                    StartCoroutine(WaitShowStep(3));
                }
                break;

            case 3:
                if (Input.GetKeyDown(KeyCode.LeftControl))//huong dan lan ve phia sau
                {
                    if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    {
                        stepCompleted[3] = true;
                        tutorialPanelV1.SetActive(false); // Ẩn panel hướng dẫn
                        StartCoroutine(WaitShowStep(4)); // Hiển thị bước tiếp theo sau 1 giây

                    }
                    
                }
                break;
            case 4:
                if (slowMotion.isDodgeWindowActive && !tutorialPanelV1.activeSelf)
                {
                    tutorialPanelV1.SetActive(true);                 
                    defautIcon.sprite = rollBackIcon;
                  
                }

                if (Input.GetKeyDown(KeyCode.LeftControl) && slowMotion.isDodgeWindowActive)
                {
                    slowMotion.ResetTime();
                    stepCompleted[4] = true;
                    playerControllerState.isAttack = true;

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
            case 5:
                
                if (Input.GetKeyDown(KeyCode.Mouse0))//attack 
                {
                    stepCompleted[5] = true;
                    StartCoroutine(WaitShowStep(6));
                }

                break;
        }
    }



    public IEnumerator WaitShowStep(int step)
    {
        animatorPanelTutorial.animator.SetTrigger("Start");//chay animator tat
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
                break;

            case 2:
                tutorialTextV1.text = "Nhấn Space để nhảy.";
                defautIcon.sprite = jumpIcon;
                break;

            case 3:
                tutorialTextV1.text = "Nhấn Ctrl để lăn về phía sau.";
                defautIcon.sprite = rollBackIcon;
                
                break;
            case 4:              
                tutorialTextV1.text = "Nhấn Ctrl để né!";
                defautIcon.sprite = rollBackIcon;
                tutorialPanelV1.SetActive(false); // Ẩn panel trước khi hiển thị bước mới
                break;
            case 5:
                tutorialTextV1.text = "Nhấn chuột trái để tấn công.";
                defautIcon.sprite = attackIcon;
                break;
        }
    }

}
