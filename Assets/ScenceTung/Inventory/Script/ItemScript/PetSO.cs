using UnityEngine;

[CreateAssetMenu(fileName = "NewPet", menuName = "Inventory/Pet")]
public class PetSO : ScriptableObject
{
    public string itemName;
    [Header("Hiển thị UI")]
    public Sprite itemSprite;


    public bool UsePet()
    {

    }
}
