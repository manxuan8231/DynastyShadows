using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class OpenInventory : MonoBehaviour
{
    public GameObject inventoryCanvas;

    public GameObject panelStatus;
    public GameObject panelInven;
    public GameObject panelSkill;

    public AudioSource audioSource;
    public AudioClip audioClipClick;

 

    void Start()
    {
        inventoryCanvas.SetActive(false);
        panelStatus.SetActive(true);
      
        panelSkill.SetActive(false);
        audioSource = GetComponent<AudioSource>();
     
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (inventoryCanvas.activeSelf) 
            {
                inventoryCanvas.SetActive(false);
                Time.timeScale = 1.0f;
                // Ẩn chuột khi tắt map
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            } 
            else 
            {
                inventoryCanvas.SetActive(true);
                Time.timeScale = 0f;
                // Hiện chuột khi mở map
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    //status
    public void OpenButtonStatus()
    {
        panelStatus.SetActive(true);
        panelInven.SetActive(false);
        panelSkill.SetActive(false);
        audioSource.PlayOneShot(audioClipClick);
    }
    //skill
    public void OpenButtonSkill()
    {
        panelSkill.SetActive(true);
        panelInven.SetActive(false);
        panelStatus.SetActive(false);
        audioSource.PlayOneShot(audioClipClick);
    }
   
}
