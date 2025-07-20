
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    public GameObject canvasPause;
    public GameObject panelEquipment;
    public GameObject panelInventory;
    public GameObject panelSkillTree;
    public GameObject panelSetting;

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
    void Start()
    {
        openSkillTree =FindAnyObjectByType<OpenSkillTree>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        audioSource = GetComponent<AudioSource>();
        canvasPause.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inventoryManager.isOpenInventory)
        {
            bool isPaused = !canvasPause.activeSelf;
            canvasPause.SetActive(isPaused);
           
            Time.timeScale = isPaused ? 0f : 1f;

            // Ẩn hoặc hiện chuột
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            //open skill tree
            openSkillTree.textFlexible.color = Color.black;
            openSkillTree.buttonFlexibleBG.enabled = true;
            openSkillTree .flexibleSkill.SetActive(true); // Hiển thị Flexible Skill khi mở Skill Tree
            openSkillTree .coreTree.SetActive(false); // Ẩn Core Tree khi mở Flexible Skill
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
    public void ButtonEquipment()
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
    public void ButtonInven()
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
    public void ButtonSkillTree() 
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
    public void ButtonSetting()
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

    public void ExitToMainMenu()
    {

    }
}
