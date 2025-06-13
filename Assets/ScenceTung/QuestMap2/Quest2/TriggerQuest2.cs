using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TriggerQuest2 : MonoBehaviour
{
    public CinemachineCamera cam1;
    public GameObject canvasText;
    public TMP_Text contentText;
    public GameObject stateCanvas;
    public TMP_Text stateQuestion;
    public TMP_Text missionName;
    public Image missionIcon;
    public Sprite missionSprite;
    public GameObject npcQuest;
    public TMP_Text contentQuest;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Show()); // Start the coroutine to show the text
        }
    }

   

    IEnumerator Show()
    {
        cam1.Priority = 11; // Set the camera priority to 11
        canvasText.SetActive(true); // Show the canvas text
        contentText.text = "Các binh sĩ ?!";
        yield return new WaitForSeconds(2.5f);
        contentText.text = "Họ đang gặp nguy hiểm!!";
        yield return new WaitForSeconds(2.5f);
        contentText.text = "Tôi đến giúp các anh đây.Hãy cố lên!!!";
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        cam1.Priority = 0; // Reset the camera priority to 9
        canvasText.SetActive(false); // Hide the canvas text
        stateCanvas.SetActive(true); // Show the state canvas
        stateQuestion.text = "Bạn vừa nhận được nhiệm vụ mới!";
        missionName.text = "Nhiệm vụ: Tiêu diệt các sinh vật ";
        missionIcon.sprite = missionSprite;
        yield return new WaitForSeconds(1.5f);
        stateCanvas.SetActive(false); // Hide the state canvas
        npcQuest.SetActive(true);
        contentQuest.text = "Hỗ trợ kỵ sĩ còn sống đẩy lùi đợt tấn công của dị thể";
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject); // Destroy the trigger object after showing the text

    }
}
