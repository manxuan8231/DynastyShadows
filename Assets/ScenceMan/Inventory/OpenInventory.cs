using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class OpenInventory : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject panelStatus;
    public GameObject panelSkill;
    public AudioSource audioSource;
    public AudioClip audioClipClick;
    public GameObject inventory;

    public ItemSlot[] itemSlot;
    
    void Start()
    {
        inventoryCanvas.SetActive(false);
        panelStatus.SetActive(true);
        inventory.SetActive(false);
        panelSkill.SetActive(false);
        audioSource = GetComponent<AudioSource>();
     
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (inventoryCanvas.activeSelf) 
            {
                inventory.SetActive(false);
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
        inventory.SetActive(false);
        panelSkill.SetActive(false);
        audioSource.PlayOneShot(audioClipClick);
    }
    //skill
    public void OpenButtonSkill()
    {
        inventory.SetActive(false);
        panelSkill.SetActive(true);
        panelStatus.SetActive(false);
        audioSource.PlayOneShot(audioClipClick);
    }
    public void OpenINV()
    {
        inventory.SetActive(true);
        panelStatus.SetActive(false);
        panelSkill.SetActive(false);
        audioSource.PlayOneShot(audioClipClick);
    }



    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemSO itemSO)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemSO);
                if (leftOverItems > 0)
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription, itemSO);
                return leftOverItems;
            }
        }
        return quantity;
    }
    public void DeselectedAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].thisItemSelected = false;
            itemSlot[i].selectPanel.SetActive(false);
        }
    }
}
