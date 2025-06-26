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
    public PlayerControllerState playerControllerState;
    public BoxCollider boxCollider;
    public AudioSource audioSource;
    public AudioClip backGround;
    public AudioClip dangerClip;
    private void Start()
    {
        playerControllerState = FindFirstObjectByType<PlayerControllerState>();
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Show()); // Start the coroutine to show the text
        }
    }

   

    IEnumerator Show()
    {
        boxCollider.enabled = false; // Disable the box collider to prevent re-triggering   
        cam1.Priority = 11; // Set the camera priority to 11
        playerControllerState.animator.enabled = false; // Disable player animations
        playerControllerState.enabled = false; // Disable player controls
        canvasText.SetActive(true); // Show the canvas text
        contentText.text = "Ở đó hình như còn có người sống sót";
        audioSource.clip = dangerClip; // Set the danger audio clip
        audioSource.Play(); // Play the danger audio
        yield return new WaitForSeconds(2.5f);
        contentText.text = "Nhưng sao lại có những sinh vật kì lạ lại giam giữ hắn ?!";
        yield return new WaitForSeconds(2.5f);
        contentText.text = "Mình phải giải cứu người đang bị chúng giam giữ...";
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        audioSource.clip = backGround; // Set the background audio clip
        audioSource.Play(); // Play the background audio
        playerControllerState.enabled = true; // Re-enable player controls
        playerControllerState.animator.enabled = true; // Re-enable player animations
        cam1.Priority = 0; // Reset the camera priority to 9
        canvasText.SetActive(false); // Hide the canvas text
        stateCanvas.SetActive(true); // Show the state canvas
        stateQuestion.text = "Bạn vừa nhận được nhiệm vụ mới!";
        missionName.text = "Nhiệm vụ: Tiêu diệt các sinh vật ";
        missionIcon.sprite = missionSprite;
        yield return new WaitForSeconds(1.5f);
        stateCanvas.SetActive(false); // Hide the state canvas
        npcQuest.SetActive(true);
        contentQuest.text = "Giải cứu dân làng bị giam giữ!!!";
        yield return new WaitForSeconds(1.5f);

    }
}
