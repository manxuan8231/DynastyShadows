using UnityEngine;
using UnityEngine.UI;

public class CraftingNPC : MonoBehaviour
{
    public GameObject craftingMenu;
    public GameObject interactUI; // UI hiện chữ "Nhấn F"
    public float interactionDistance = 3f;
    private GameObject player;
    private bool isPlayerInRange = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (interactUI != null)
            interactUI.SetActive(false); // tắt UI lúc đầu
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        isPlayerInRange = distance <= interactionDistance;

        // Hiện UI khi ở trong tầm
        if (interactUI != null)
            interactUI.SetActive(isPlayerInRange && !craftingMenu.activeSelf);

        // Nếu trong tầm và nhấn F -> mở menu
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OpenCraftingMenu();
        }

        // Nhấn X để đóng menu
        if (Input.GetKeyDown(KeyCode.X))
        {
            craftingMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OpenCraftingMenu()
    {
        if (craftingMenu != null)
        {
            craftingMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (interactUI != null)
                interactUI.SetActive(false); // tắt UI khi đã mở menu
        }
    }
}
