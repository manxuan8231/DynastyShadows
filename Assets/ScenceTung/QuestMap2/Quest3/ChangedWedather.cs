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

    void Start()
    {
        turnInQuest2Map2 = FindAnyObjectByType<TurnInQuest2Map2>();
        boxCollider = GetComponent<BoxCollider>();
        _light = GameObject.Find("Directional Light(None)").GetComponent<Light>(); // Tìm ánh sáng trong cảnh

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
        if (btnF.activeSelf && Input.GetKeyDown(KeyCode.F) && !isBtnOpen)
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
        yield return null;
        _light.color = Color.black;
        _light.intensity = 0.5f; // Giảm độ sáng của ánh sáng để tạo hiệu ứng thời tiết tối hơn
        RenderSettings.fog = true; // Bật sương mù
        RenderSettings.fogDensity = 0.01f; //tăng độ mờ của sương mù
        Debug.Log("Đã thay đổi thời tiết!"); // Thay đổi thời tiết ở đây
                                             // 👉 ĐỔI SKYBOX
        if (skybox != null)
        {
            RenderSettings.skybox = skybox;
            DynamicGI.UpdateEnvironment(); // Cập nhật lighting để khớp Skybox mới
        }


    }


}
   
