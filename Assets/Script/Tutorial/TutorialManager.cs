using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanelV1;
    public TMP_Text tutorialTextV1;

    private int currentStep = 0;
    private bool[] stepCompleted;

    private SlowMotionDodgeEvent slowMotion;

    void Start()
    {
        stepCompleted = new bool[5];
        ShowStep(0);
        slowMotion = FindAnyObjectByType<SlowMotionDodgeEvent>();
    }

    void Update()
    {
        switch (currentStep)
        {
            case 0:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
                    || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))//huong dan di chuyen
                {
                    stepCompleted[0] = true;
                    ShowStep(1);
                }
                break;

            case 1:
                if (Input.GetKeyDown(KeyCode.LeftControl) &&
                    (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) 
                  || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))//huong dan lan ve phia trc
                {
                    stepCompleted[1] = true;
                    ShowStep(2);
                }
                break;

            case 2:
                if (Input.GetKeyDown(KeyCode.LeftControl))//huong dan lan ve phia sau
                {
                    stepCompleted[2] = true;
                    ShowStep(3);
                    tutorialPanelV1.SetActive(false); //ẩn panel chờ quái danh 
                }
                break;

            case 3:
                // Khi cửa sổ né đòn đang mở, hiện panel ngay
                if (slowMotion.isDodgeWindowActive && !tutorialPanelV1.activeSelf)//huong dan ne attack
                {
                    tutorialPanelV1.SetActive(true);
                    tutorialTextV1.text = "Chuẩn bị bị tấn công! Nhấn Ctrl để né!";
                }

                // Khi người chơi né đúng
                if (Input.GetKeyDown(KeyCode.LeftControl) && slowMotion.isDodgeWindowActive)
                {
                    slowMotion.ResetTime();
                    stepCompleted[3] = true;
                    ShowStep(4);
                }
                break;

            case 4:
                tutorialPanelV1.SetActive(true);
                tutorialTextV1.text = "Hoàn thành hướng dẫn!";
                break;
        }
    }

    void ShowStep(int step)
    {
        currentStep = step;

        // Các bước từ 0 den 2 sẽ hiển thị panel
        if (step >= 0 && step <= 2)
        {
            tutorialPanelV1.SetActive(true);
        }

        switch (step)
        {
            case 0:
                tutorialTextV1.text = "Dùng W/A/S/D để di chuyển.";
                break;

            case 1:
                tutorialTextV1.text = "Nhấn Ctrl + W/A/S/D để lăn về phía trước.";
                break;

            case 2:
                tutorialTextV1.text = "Nhấn Ctrl để lăn về phía sau.";
                break;

            case 3:
                // để trong Update khi slowMotion kích hoạt
                break;

            case 4:
                // Đã xử lý trong Update 
                break;
        }
    }
}
