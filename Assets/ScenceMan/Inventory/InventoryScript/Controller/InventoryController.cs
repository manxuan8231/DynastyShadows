using UnityEngine;
using UnityEngine.Audio;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;

    public int inventorySize = 20;
    private void Start()
    {
        inventoryUI.InitializeInventoryUI(inventorySize);
        
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
              
            }
            else
            {
                inventoryUI.Hide();
            }

        }
    }
}
