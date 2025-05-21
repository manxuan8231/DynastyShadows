using UnityEngine;

public class ThongTinIcon : MonoBehaviour
{
    public GameObject panelIcon;//ng xem thong tin
    public AudioSource audioSource;
    public AudioClip clipClick;

    public GameObject clickBG;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        panelIcon.SetActive(false); // Ẩn nút teleport khi bắt đầu
        clickBG.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            panelIcon.SetActive(false); // Ẩn nút teleport khi không còn click vào map
            clickBG.SetActive(false );
        }
    }
    public void ShowPanelIcon()
    {
        panelIcon.SetActive(true); // Hiện nút teleport khi click vào map
        audioSource.PlayOneShot(clipClick);
    }
    public void CancelPanelIcon()
    {
        panelIcon.SetActive(false); // Ẩn nút teleport khi không còn click vào map
       // audioSource.PlayOneShot(clipClick);
    }

}
