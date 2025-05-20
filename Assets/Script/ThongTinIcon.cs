using UnityEngine;

public class ThongTinIcon : MonoBehaviour
{
    public GameObject panelIcon;
    void Start()
    {
        panelIcon.SetActive(false); // Ẩn nút teleport khi bắt đầu
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            panelIcon.SetActive(false); // Ẩn nút teleport khi không còn click vào map
        }
    }
    public void ShowPanelIcon()
    {
        panelIcon.SetActive(true); // Hiện nút teleport khi click vào map
    }
    public void CancelPanelIcon()
    {
        panelIcon.SetActive(false); // Ẩn nút teleport khi không còn click vào map
    }

}
