using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;


    public void Awake()
    {
        ResetDescription();
    }
    public void ResetDescription()
    {

        this.itemImage.gameObject.SetActive(false);
        this.title.text = "";
        this.description.text = "";

    }
    public void SetDescription(Sprite sprite, string title, string description)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
    }
}
