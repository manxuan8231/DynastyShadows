using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenSkillTree : MonoBehaviour
{
    public GameObject panelSkillTree;
    public GameObject pauseManager;
    // Các đối tượng Skill Tree
    // Flexible Skill
    public GameObject flexibleSkill;
    public TextMeshProUGUI textFlexible;
    public Image buttonFlexibleBG;
    // Core Tree
    public Image buttonCoreBG;
    public GameObject coreTree;
    public TextMeshProUGUI textCore;
    //panel inven an no
    public GameObject panelInven;
    public GameObject panelInvenEq;
    public GameObject buttonInven;
    public bool isOpenSkillTree = true;
    public AudioSource audioSource; // Thêm biến AudioSource để phát âm thanh
    public AudioClip openSkillTreeSound; // Thêm biến AudioClip để chứa âm thanh mở Skill Tree
    public AudioClip clickButtonSound;
    void Start()
    {
           panelSkillTree.SetActive(false);
        buttonCoreBG.enabled = false;
        // flexibleSkill.SetActive(false);

        coreTree.SetActive(false);    
            audioSource = GetComponent<AudioSource>(); // Lấy AudioSource từ GameObject này
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && isOpenSkillTree) // Nhấn phím T để mở/đóng Skill Tree
        {
            bool willOpen = !panelSkillTree.activeSelf; // Trạng thái sau khi nhấn

            panelSkillTree.SetActive(willOpen);
            pauseManager.SetActive(willOpen);
            audioSource.PlayOneShot(openSkillTreeSound);
            Cursor.lockState = willOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = willOpen;
            Time.timeScale = willOpen ? 0.0f : 1.0f;//
            
            // Mặc định mở Flexible Tree
            flexibleSkill.SetActive(true);
            buttonFlexibleBG.enabled = true;
            textFlexible.color = Color.black;
            // Mặc định mở core Tree
            coreTree.SetActive(false);
            buttonCoreBG.enabled = false;
            textCore.color = Color.white;

            //inventory tat moi khi mo skill treee
            panelInven.SetActive(false);
            panelInvenEq.SetActive(false);
            buttonInven.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
        {
            panelSkillTree.SetActive(false);
            flexibleSkill.SetActive(false);
            coreTree.SetActive(false);
        }

    }
    // Mở skill da dang  khi nhấn nút trên giao diện
    public void ButtonFlexibleSkill()
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút Flexible Skill
        buttonFlexibleBG.enabled = true; // bat nền của nút Flexible Skill
        buttonCoreBG.enabled = false; // ẩn nền của nút Core Tree
        textFlexible.color = Color.black;
        textCore.color = Color.white; // Đổi màu chữ Core Tree về màu trắng
        flexibleSkill.SetActive(true); // Hiển thị Flexible Skill khi mở Skill Tree
        coreTree.SetActive(false); // Ẩn Core Tree khi mở Flexible Skill
    }
  

    // Mở skill cung khi nhấn nút trên giao diện
    public void ButtonCoreTree()
    {
        audioSource.PlayOneShot(clickButtonSound); // Phát âm thanh khi nhấn nút Flexible Skill
        buttonCoreBG.enabled = true; // Bật nền của nút Core Tree
        buttonFlexibleBG.enabled = false; // Ẩn nền của nút Flexible Skill
        textCore.color = Color.black;
        textFlexible.color = Color.white; // Đổi màu chữ Flexible Skill về màu trắng
        coreTree.SetActive(true);
        flexibleSkill.SetActive(false); // Ẩn Flexible Skill khi mở Core Tree
    }
   
}
