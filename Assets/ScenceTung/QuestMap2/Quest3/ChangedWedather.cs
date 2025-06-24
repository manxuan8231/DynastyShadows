using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangedWedather : MonoBehaviour
{
    public TurnInQuest2Map2 turnInQuest2Map2;
    public GameObject canvasContent;
    public GameObject btnF;
    public TMP_Text btnText;
    public BoxCollider boxCollider;
    public Light _light; // Giả sử bạn có một ánh sáng hướng để thay đổi thời tiết
    public bool isBtnOpen = false;
    public GameObject effect;
    public Material skybox;
    public Material skyboxOgirnal;
    public bool isAudioActive = false;
    public bool isAudioDoneQuest = false;
    public bool isDoneQuest3 = false;
    public AudioSource AudioSource;
    public AudioClip clip1;



    void Start()
    {

        turnInQuest2Map2 = FindAnyObjectByType<TurnInQuest2Map2>();
        boxCollider = GetComponent<BoxCollider>();
        _light = GameObject.Find("Directional Light(None)").GetComponent<Light>(); // Tìm ánh sáng trong cảnh
        AudioSource = GetComponent<AudioSource>();


    }
    private void Update()
    {
        
    }

   void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && turnInQuest2Map2.isQuest2 == true && !isBtnOpen)
        {
            canvasContent.SetActive(true);
            btnF.gameObject.SetActive(true);
            btnText.text = "F :Chạm";

        }
        if (canvasContent.activeSelf && btnF.activeSelf && Input.GetKeyDown(KeyCode.F) && !isBtnOpen)
        {
            isBtnOpen = true; // Đặt cờ để biết nút đã được nhấn
            StartCoroutine(changedWeather());
            btnF.SetActive(false); // Ẩn nút sau khi nhấn
            boxCollider.enabled = false; // Vô hiệu hóa collider để không thể kích hoạt lại
            effect.SetActive(false); // Kích hoạt hiệu ứng
        }

    }
    IEnumerator changedWeather()
    {
        yield return new WaitForSeconds(1f) ;
        // 👉 THAY ĐỔI THỜI TIẾT
        _light.color = Color.black;
        _light.intensity = 0.2f; // Giảm độ sáng của ánh sáng để tạo hiệu ứng thời tiết tối hơn
        RenderSettings.fog = true; // Bật sương mù
        RenderSettings.fogDensity = 0.01f; //tăng độ mờ của sương mù
        Debug.Log("Đã thay đổi thời tiết!"); // Thay đổi thời tiết ở đây
        
        if (skybox != null)// 👉 ĐỔI SKYBOX
        {
            RenderSettings.skybox = skybox;
            DynamicGI.UpdateEnvironment(); // Cập nhật lighting để khớp Skybox mới
        }
        if(!isAudioActive)
        {
            AudioSource.clip = clip1;
            AudioSource.Play();
            AudioSource.loop = true; // Đặt âm thanh lặp lại
            isAudioActive = true; // Đặt cờ để biết âm thanh đã được phát
        }
        if (isAudioDoneQuest)
        {
            AudioSource.Stop(); // Dừng âm thanh nếu đã phát
            isAudioDoneQuest = false; // Đặt lại cờ
        }
        yield return new WaitForSeconds(3f);
        isDoneQuest3 = true; // Đặt cờ để biết nhiệm vụ đã hoàn thành

    }

    public void ChangedFirstWeather()
    {
        // 👉 THAY ĐỔI THỜI TIẾT
        if (skyboxOgirnal != null)// 👉 ĐỔI SKYBOX
        {
            RenderSettings.skybox = skyboxOgirnal;
            DynamicGI.UpdateEnvironment(); // Cập nhật lighting để khớp Skybox mới
        }
        StartCoroutine(TimeLine());

    }
   IEnumerator TimeLine() {
        _light.color = new Color(1, 0.6430022f, 0.1911765f, 1);
        _light.intensity = 0.2f; // Đặt độ sáng của ánh sáng về mức ban đầu
        yield return new WaitForSeconds(0.5f);
        _light.intensity = 0.35f; // Tăng độ sáng của ánh sáng
        yield return new WaitForSeconds(0.5f);
        _light.intensity = 0.5f; // Tăng độ sáng của ánh sáng
        yield return new WaitForSeconds(0.5f);
        _light.intensity = 0.7f; // Tăng độ sáng của ánh sáng
        yield return new WaitForSeconds(0.5f);
        _light.intensity = 1f; // Tăng độ sáng của ánh sáng
        yield
            return new WaitForSeconds(0.5f);
        _light.intensity = 1.5f; // Tăng độ sáng của ánh sáng
        yield return new WaitForSeconds(0.5f);
        _light.intensity = 2f; // Tăng độ sáng của ánh sáng
        RenderSettings.fog = false; // Tắt sương mù
        isAudioActive = true;
        AudioSource.Stop(); // Dừng âm thanh nếu đã phát



    }

}
   
