using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OpenSkillTree : MonoBehaviour
{
    public GameObject panelSkillTree;
    public GameObject canvasManager;
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
    public TextMeshProUGUI enoughtSkill;// thong bao đủ điểm nâng cấp kỹ năng
    //tham chieu
    public PauseManager pauseManager;
    public PlayerControllerState playerControllerState; // Tham chiếu đến PlayerControllerState
    public PlayerStatus playerStatus;
    public OpenMap openMap; // Tham chiếu đến OpenMap để kiểm tra trạng thái mở bản đồ

    void Start()
    {
        pauseManager = FindAnyObjectByType<PauseManager>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        openMap = FindAnyObjectByType<OpenMap>();
        panelSkillTree.SetActive(false);
        buttonCoreBG.enabled = false;
        enoughtSkill.enabled = false;
        // flexibleSkill.SetActive(false);

        coreTree.SetActive(false);    
            audioSource = GetComponent<AudioSource>(); // Lấy AudioSource từ GameObject này

       
          
        
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && isOpenSkillTree &&playerControllerState.animator.enabled && !openMap.isTurnOffMap) // Nhấn phím T để mở/đóng Skill Tree
        {
            bool willOpen = !panelSkillTree.activeSelf; // Trạng thái sau khi nhấn

            panelSkillTree.SetActive(willOpen);
            canvasManager.SetActive(willOpen);
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

            //pause manager
            //img
            pauseManager.imageEquipment.enabled = false;
            pauseManager.imageInventory.enabled = false;
            pauseManager.imageSetting.enabled = false;
            pauseManager.imageSkillTree.enabled = true;
            //text
            pauseManager.textEquipment.color = Color.white;
            pauseManager.textInventory.color = Color.white;
            pauseManager.textSkillTree.color = Color.black;
            pauseManager.textSetting.color = Color.white;
            //panel
            pauseManager.panelInventory.SetActive(false);
        
            pauseManager.panelSetting.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape))
        {
            enoughtSkill.enabled = false;
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

    //đủ level thì hiện thông báo
    public IEnumerator WaitOffEnought()
    {
        enoughtSkill.enabled = true;    
        yield return new WaitForSeconds(4f);
        enoughtSkill.enabled = false;
    }
}
