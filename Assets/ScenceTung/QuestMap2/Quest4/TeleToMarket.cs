using System.Collections;
using UnityEngine;

public class TeleToMarket : MonoBehaviour
{
    public Transform telePos;
    public GameObject loadingPanel; // OBJ cần gán trong inspector
    public Transform player; // OBJ cần gán trong inspector
    PlayerControllerState playerController; // Tham chiếu đến PlayerController script
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack script
    public GameObject questPoint;
    public bool isTeleDone = false;
    public BoxCollider boxCollider; // BoxCollider để kiểm tra va chạm
    private void Start()
    {
        boxCollider.enabled = false;
        playerController = FindAnyObjectByType<PlayerControllerState>(); // Tìm đối tượng PlayerController trong scene
        comboAttack = FindAnyObjectByType<ComboAttack>(); // Tìm đối tượng ComboAttack trong scene
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("SK_DeathKnight");
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
        if(other.tag == "Player")
        {
            StartCoroutine(TelePort());
        }
    }


    IEnumerator TelePort()
    {
        boxCollider.enabled = false; // Tắt collider để không thể kích hoạt lại dịch chuyển
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
        playerController.animator.SetBool("isWalking", false);
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
            player.transform.position = telePos.position;
            cc.enabled = true;
        }
        // Tắt loading panel
        if (loadingPanel != null)
        loadingPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        isTeleDone = true; // Đặt cờ để biết nhiệm vụ đã hoàn thành
        questPoint.SetActive(false); // Kích hoạt questPoint sau khi dịch chuyển
    }


}

