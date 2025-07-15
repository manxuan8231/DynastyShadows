
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
    void Start()
    {
        openSkillTree =FindAnyObjectByType<OpenSkillTree>();
        canvasPause.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !canvasPause.activeSelf;
            canvasPause.SetActive(isPaused);
           
            Time.timeScale = isPaused ? 0f : 1f;

            // Ẩn hoặc hiện chuột
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            openSkillTree.textFlexible.color = Color.black;
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
}
