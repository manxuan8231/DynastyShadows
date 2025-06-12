using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CamTrigger : MonoBehaviour
{
    public CinemachineCamera cam1;
    public CinemachineCamera cam2;
    public CinemachineCamera cam3;
    public CinemachineCamera cam4;
    public CinemachineBrain cinemachineBrain;

    public PlayerControllerState playerControllerState;
    public GameObject triggerCam;
    public GameObject canvasText;
    public TMP_Text content;
    public GameObject stateCanvas;
    public TMP_Text stateQuestion;
    public TMP_Text missionName;
    public Image missionIcon;
    public Sprite missionSprite;
    public GameObject canvasQuest;
    public AudioCanvasState audioCanvasState;
    
    private void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CameraChanged());
        }
    }

    IEnumerator CameraChanged()
    {
        playerControllerState.enabled = false;
        playerControllerState.animator.enabled = false;
        canvasText.SetActive(true);

       
        CinemachineBlendDefinition blendDefinition = new CinemachineBlendDefinition
        {
            Style = CinemachineBlendDefinition.Styles.Cut
        };
        cinemachineBrain.DefaultBlend = blendDefinition;

        content.text = "Bọn quái vật đã phá hủy mất một phần thị trấn rồi sao?....";
        cam1.Priority = 20;
        yield return new WaitForSeconds(5f);
        content.text = "Chúng ta phải nhanh chóng tìm cách tiêu diệt chúng trước khi chúng phá hủy hết mọi thứ!";
        cam2.Priority = 20;
        cam1.Priority = 0;
        yield return new WaitForSeconds(5f);
        content.text = "Binh lính đã cố gắng.Nhưng....!";
        cam3.Priority = 20;
        cam2.Priority = 0;
        yield return new WaitForSeconds(5f);
        content.text = "Khung cảnh hiện tại thật tàn khóc...!";
        cam4.Priority = 20;
        cam3.Priority = 0;
        yield return new WaitForSeconds(5f);
        cam4.Priority = 0;
        playerControllerState.enabled = true;
        playerControllerState.animator.enabled = true;
        cam4.Priority = 0;
        CinemachineBlendDefinition defaultCam = new CinemachineBlendDefinition
        {
            Style = CinemachineBlendDefinition.Styles.EaseInOut
        };
        cinemachineBrain.DefaultBlend = defaultCam;
        canvasText.SetActive(false);
        yield return new WaitForSeconds(1f);
        stateCanvas.SetActive(true);
        stateQuestion.text = "Bạn vừa nhận nhiệm vụ mới !";
        missionName.text = "Nhiệm vụ: Đi xung quanh khu bị phá hủy kiểm tra...!";
       missionIcon.sprite = missionSprite;
        audioCanvasState.PlayNewQuest();
        yield return new WaitForSeconds(4.5f);
        stateCanvas.SetActive(false);
        yield return new WaitForSeconds(1f);
        canvasQuest.SetActive(true);
        Destroy(triggerCam);
    }
}
