using UnityEngine;

public class CraftingNPC : MonoBehaviour
{
    public GameObject craftingMenu;
    public float interactionDistance = 3f;
    private GameObject player;
    private bool isPlayerInRange = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        isPlayerInRange = distance <= interactionDistance;

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.C))
        {
            OpenCraftingMenu();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
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
        }
    }
}
