using System.Collections;
using UnityEngine;

public class TeleToSafeZone : MonoBehaviour
{
    public Transform telePos;
    public GameObject loadingPanel; // OBJ cần gán trong inspector
    public Transform player; // OBJ cần gán trong inspector
    PlayerControllerState playerController; // Tham chiếu đến PlayerController script
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack script
    public bool isTeleDone = false;
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerControllerState>(); // Tìm đối tượng PlayerController trong scene
        comboAttack = FindAnyObjectByType<ComboAttack>(); // Tìm đối tượng ComboAttack trong scene
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("SK_DeathKnight(Clone)");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (loadingPanel == null && player != null)
        {
            Transform canvasLoadTransform = player.transform.Find("CanvasLoad");
            if (canvasLoadTransform != null)
            {
                loadingPanel = canvasLoadTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy CanvasLoad trong Player.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(TelePort());
        }
    }


    IEnumerator TelePort()
    {
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
        playerController.animator.enabled = false; // Tắt Animator để tránh lỗi khi dịch chuyển
        // Đợi 5 giây
        yield return new WaitForSeconds(5f);
        playerController.enabled = true; // Bật lại PlayerController
        comboAttack.enabled = true; // Bật lại ComboAttack

        // Dịch chuyển nhân vật
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = telePos.position;
            cc.enabled = true;
        }
        // Tắt loading panel
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        playerController.animator.enabled = true; // Bật lại Animator
        isTeleDone = true; // Đặt cờ để biết nhiệm vụ đã hoàn thành
        gameObject.SetActive(false); // Ẩn đối tượng này sau khi hoàn thành nhiệm vụ
    }
}
