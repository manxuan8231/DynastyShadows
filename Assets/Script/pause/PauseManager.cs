
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    public GameObject canvasPause;
    public GameObject panelEquipment;
    public GameObject panelInventory;
    public GameObject panelSkillTree;
    public GameObject panelSetting;
    public GameObject panelQuitGame;
    //imgButton cho dep
    public Image imageEquipment;
    public Image imageInventory;
    public Image imageSkillTree;
    public Image imageSetting;
    //text button
    public TextMeshProUGUI textEquipment;
    public TextMeshProUGUI textInventory;
    public TextMeshProUGUI textSkillTree;
    public TextMeshProUGUI textSetting;

    //tham chieu
    private OpenSkillTree openSkillTree;
    public AudioSource audioSource; // Thêm biến AudioSource để phát âm thanh
    public AudioClip clickButtonSound; // Thêm biến AudioClip để chứa âm thanh khi nhấn nút
    public InventoryManager inventoryManager; // Thêm biến InventoryManager để quản lý kho
    public PlayerControllerState playerController; // Tham chiếu đến PlayerController
    public OpenMap openMap; // Tham chiếu đến OpenMap để kiểm tra trạng thái mở bản đồ
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerControllerState>();
        openSkillTree =FindAnyObjectByType<OpenSkillTree>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        openMap = FindAnyObjectByType<OpenMap>();
        audioSource = GetComponent<AudioSource>();
        canvasPause.SetActive(false);
        panelQuitGame.SetActive(false);

        // Khi mới load scene game, tắt chuột, cho game chạy bình thường
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }


    void Update()
    {
        TurnOnOffPause();
    }
    public void TurnOnOffPause()//tat bat pause
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryManager.isOpenInventory && playerController.animator.enabled
            && !openMap.isTurnOffMap && TurnOffOnUI.openShop == false && !TurnOffOnUI.isTutorialInven)
        {
            bool isPaused = !canvasPause.activeSelf;
            canvasPause.SetActive(isPaused);
            TurnOffOnUI.pause = isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            panelQuitGame.SetActive(false);
            // Ẩn hoặc hiện chuột
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            //open skill tree
            openSkillTree.textFlexible.color = Color.black;
            openSkillTree.buttonFlexibleBG.enabled = true;
            openSkillTree.flexibleSkill.SetActive(true); // Hiển thị Flexible Skill khi mở Skill Tree
            openSkillTree.coreTree.SetActive(false); // Ẩn Core Tree khi mở Flexible Skill
            openSkillTree.textCore.color = Color.white;
            openSkillTree.buttonCoreBG.enabled = false;

            //img
            imageEquipment.enabled = isPaused;
            imageInventory.enabled = false;
            imageSetting.enabled = false;
            imageSkillTree.enabled = false;
            //text
            textEquipment.color = Color.black;
            textInventory.color = Color.white;
            textSkillTree.color = Color.white;
            textSetting.color = Color.white;
            //panel
            panelEquipment.SetActive(isPaused);
            panelInventory.SetActive(false);
            panelSkillTree.SetActive(false);
            panelSetting.SetActive(false);

        }
        
    }
    public void TurnOnPause()//bat pause
    {
       
        canvasPause.SetActive(true);
        TurnOffOnUI.pause = true;
        Time.timeScale = 0f;
        panelQuitGame.SetActive(false);
        // Ẩn hoặc hiện chuột
        Cursor.visible = true;
        Cursor.lockState =  CursorLockMode.None;
        //open skill tree
        openSkillTree.textFlexible.color = Color.black;
        openSkillTree.buttonFlexibleBG.enabled = true;
        openSkillTree.flexibleSkill.SetActive(true); // Hiển thị Flexible Skill khi mở Skill Tree
        openSkillTree.coreTree.SetActive(false); // Ẩn Core Tree khi mở Flexible Skill
        openSkillTree.textCore.color = Color.white;
        openSkillTree.buttonCoreBG.enabled = false;

        //img
        imageEquipment.enabled = true;
        imageInventory.enabled = false;
        imageSetting.enabled = false;
        imageSkillTree.enabled = false;
        //text
        textEquipment.color = Color.black;
        textInventory.color = Color.white;
        textSkillTree.color = Color.white;
        textSetting.color = Color.white;
        //panel
        panelEquipment.SetActive(true);
        panelInventory.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
    }

    public void ButtonEquipment()//bat trang bi
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút
        //img
        imageEquipment.enabled = true;
        imageInventory.enabled = false;
        imageSetting.enabled = false;
       imageSkillTree.enabled = false;
        //text
        textEquipment.color = Color.black;
        textInventory.color = Color.white;
        textSkillTree.color = Color.white;
        textSetting.color = Color.white;
        //panel
        panelEquipment.SetActive(true);
        panelInventory.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonInven() //bat inven
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút
        //img
        imageEquipment.enabled = false;
        imageInventory.enabled = true;
        imageSetting.enabled = false;
        imageSkillTree.enabled = false;
        //text
        textEquipment.color = Color.white;
        textInventory.color = Color.black;
        textSkillTree.color = Color.white;
        textSetting.color = Color.white;
        //panel
        panelInventory.SetActive(true);
        panelEquipment.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonSkillTree()//bat skill tree 
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút
        //img
        imageEquipment.enabled = false;
        imageInventory.enabled = false;
        imageSetting.enabled = false;
        imageSkillTree.enabled = true;
        //text
        textEquipment.color = Color.white;
        textInventory.color = Color.white;
        textSkillTree.color = Color.black;
        textSetting.color = Color.white;
        //panel
        panelSkillTree.SetActive(true);
        panelEquipment.SetActive(false);
        panelInventory.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonSetting()//bat setting
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút
        //img
        imageEquipment.enabled = false;
        imageInventory.enabled = false;
        imageSetting.enabled = true;
        imageSkillTree.enabled = false;
        //text
        textEquipment.color = Color.white;
        textInventory.color = Color.white;
        textSkillTree.color = Color.white;
        textSetting.color = Color.black;
        //panel
        panelSetting.SetActive(true);
        panelSkillTree.SetActive(false);
        panelInventory.SetActive(false);
        panelEquipment.SetActive(false);
    }

    public void PanelQuitGame()//bật cảnh báo hiện quitgame
    {
        panelQuitGame.SetActive(true);
    }
    public void QuitPanel()//tắt cảnh báo hiện quitgame
    {
        panelQuitGame.SetActive(false);
    }
    public void ExitToMainMenu()  // Quay về Main Menu
    {
       
        canvasPause.SetActive(false);

        Time.timeScale =   1f;

        // Ẩn hoặc hiện chuột
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //open skill tree
        openSkillTree.textFlexible.color = Color.black;
        openSkillTree.buttonFlexibleBG.enabled = true;
        openSkillTree.flexibleSkill.SetActive(true); // Hiển thị Flexible Skill khi mở Skill Tree
        openSkillTree.coreTree.SetActive(false); // Ẩn Core Tree khi mở Flexible Skill
        openSkillTree.textCore.color = Color.white;
        openSkillTree.buttonCoreBG.enabled = false;

        //img
        imageEquipment.enabled = false;
        imageInventory.enabled = false;
        imageSetting.enabled = false;
        imageSkillTree.enabled = false;
        //text
        textEquipment.color = Color.black;
        textInventory.color = Color.white;
        textSkillTree.color = Color.white;
        textSetting.color = Color.white;
        //panel
        panelEquipment.SetActive(false);
        panelInventory.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
