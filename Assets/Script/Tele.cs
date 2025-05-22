using UnityEngine;
using System.Collections;

public class Tele : MonoBehaviour
{
    public Transform teleportLocation;
    public GameObject loadingPanel; // Gán Panel loading ở đây
    public GameObject bgClickicon;//bg khi click vào iconmap

    OpenMap openMap; // Tham chiếu đến OpenMap script
    PlayerController playerController; // Tham chiếu đến PlayerController script
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack script

    public GameObject panelButtonTele;// Nút teleport
    void Start()
    {
        bgClickicon.SetActive(false); // Ẩn hiệu ứng khi click vào iconmap
        playerController = FindAnyObjectByType<PlayerController>(); // Tìm đối tượng PlayerController trong scene
        comboAttack = FindAnyObjectByType<ComboAttack>(); // Tìm đối tượng ComboAttack trong scene
        openMap = FindAnyObjectByType<OpenMap>(); // Tìm đối tượng OpenMap trong scene
        panelButtonTele.SetActive(false); // Ẩn nút teleport khi bắt đầu
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            panelButtonTele.SetActive(false); // Ẩn nút teleport khi không còn click vào map
            bgClickicon.SetActive(false); // Ẩn hiệu ứng khi click vào iconmap
        }
    }
    public void ShowButtonTele()
    {
        panelButtonTele.SetActive(true); // Hiện nút teleport khi click vào map
        bgClickicon.SetActive(true); // Hiện hiệu ứng khi click vào iconmap
        
    }
    public void TeleportPlayer()
    {
        bgClickicon.SetActive(false); // an hiệu ứng khi click vào iconmap
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
        bgClickicon.SetActive(false); // Ẩn hiệu ứng khi click vào iconmap
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
        {
            loadingPanel.SetActive(true);
        }
        playerController.enabled = false; // Tắt PlayerController
        comboAttack.enabled = false; // Tắt ComboAttack
        playerController. animator.SetBool("isWalking", false);
        playerController.animator.SetBool("isRunning", false);

        // Đợi 5 giây
        yield return new WaitForSeconds(5f);
        playerController.enabled = true; // Bật lại PlayerController
        
        comboAttack.enabled = true; // Bật lại ComboAttack
       
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
