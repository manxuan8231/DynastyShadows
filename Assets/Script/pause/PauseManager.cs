using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject canvasPause;
    public GameObject panelEquipment;
    public GameObject panelInventory;
    public GameObject panelSkillTree;
    public GameObject panelSetting;


    void Start()
    {
        canvasPause.SetActive(false);
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !canvasPause.activeSelf;
            canvasPause.SetActive(isPaused);
            panelEquipment.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;

            // Ẩn hoặc hiện chuột
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

            
            panelInventory.SetActive(false);
            panelSkillTree.SetActive(false);
            panelSetting.SetActive(false);
        }
        
    }
    public void ButtonEquipment()
    {
        panelEquipment.SetActive(true);
        panelInventory.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonInven()
    {
        panelInventory.SetActive(true);
        panelEquipment.SetActive(false);
        panelSkillTree.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonSkillTree() 
    {
        panelSkillTree.SetActive(true);
        panelEquipment.SetActive(false);
        panelInventory.SetActive(false);
        panelSetting.SetActive(false);
    }
    public void ButtonSetting()
    {
        panelSetting.SetActive(true);
        panelSkillTree.SetActive(false);
        panelInventory.SetActive(false);
        panelEquipment.SetActive(false);
    }
}
