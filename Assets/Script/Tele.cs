using UnityEngine;
using System.Collections;

public class Tele : MonoBehaviour
{
    public Transform teleportLocation;
    public GameObject loadingPanel; // Gán Panel loading ở đây

    OpenMap openMap; // Tham chiếu đến OpenMap script

    public GameObject panelButtonTele;
    void Start()
    {
        openMap = FindAnyObjectByType<OpenMap>(); // Tìm đối tượng OpenMap trong scene
        panelButtonTele.SetActive(false); // Ẩn nút teleport khi bắt đầu
    }
    public void ShowButtonTele()
    {
        panelButtonTele.SetActive(true); // Hiện nút teleport khi click vào map
        

    }
    public void TeleportPlayer()
    {
        panelButtonTele.SetActive(false); // Ẩn nút teleport khi không còn click vào map
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && teleportLocation != null)
        {
            StartCoroutine(TeleportAfterDelay(player));
        }
    }
    public void CancelTeleport()
    {
        panelButtonTele.SetActive(false); // Ẩn nút teleport khi không còn click vào map
    }
    IEnumerator TeleportAfterDelay(GameObject player)
    {
        openMap.mapUIBG.SetActive(false); // Tắt map UI nếu nó đang mở
        openMap.mapUIConten.SetActive(false); // Tắt map UI nếu nó đang mở
        Time.timeScale = 1f;
        // Ẩn chuột khi tắt map
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // Hiện loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
        
        // Đợi 5 giây
        yield return new WaitForSeconds(3f);

        // Dịch chuyển nhân vật
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = teleportLocation.position;
            cc.enabled = true;
        }

        // Tắt loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}
