using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
 using TMPro;

public class ActiveQuest : MonoBehaviour
{
  public CinemachineCamera camTimeLine;
  public GameObject canvasTextHN;
  public TMP_Text textHN;
  public BoxCollider boxCollider;
  public PlayerControllerState playerControllerState;

    //audio 
    public AudioSource audioSource;
    public AudioClip clip1;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(ActiveQuest3());
        }
    }
   IEnumerator ActiveQuest3(){
    boxCollider.enabled = false;
    playerControllerState.enabled = false;
    playerControllerState.animator.enabled = false;
    camTimeLine.Priority = 11;
    yield return new WaitForSeconds(1f);
    audioSource.clip = clip1;
    audioSource.Play();
    yield return new WaitForSeconds(1f);
    canvasTextHN.SetActive(true);
    textHN.text = "Sao lại có luồng khí như vậy?";
    yield return new WaitForSeconds(5f);
    textHN.text = "Thứ này là gì? Khí tức phát ra từ nó thật lạ lùng, như thứ gì đó bị phong ấn đã lâu";
    yield return new WaitForSeconds(5f);
    textHN.text = "Mình cảm thấy không ổn. Có lẽ đây là nguyên nhân khiến sinh vật trong vùng trở nên hỗn loạn";
    yield return new WaitForSeconds(0.1f);
    playerControllerState.enabled = true;
    playerControllerState.animator.enabled = true;
    yield return new WaitForSeconds(5f);
    camTimeLine.Priority = 0;
    canvasTextHN.SetActive(false);
   }
}
