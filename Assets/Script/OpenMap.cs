using UnityEngine;
using UnityEngine.UI;

public class OpenMap : MonoBehaviour
{
    public GameObject mapUIConten;
    public GameObject mapUIBG;
    public AudioSource mapAudio;
    public AudioClip mapClip;
    public bool isOpenMap;
    public bool isTurnOffMap = false; // Biến để kiểm tra xem có tắt map hay không
    //scroll map
    public ScrollRect scrollMap;
    public float scrollMapVertical;
    public float scrollMapHorizontal;
 
    //--------------------------------  nut ở dưới phải false ms cho bật map
    public GameObject panelSkillTree;//panel skill tree
    public GameObject imageBGButtonInven;//image bg button inven
    public GameObject EquipMentMenu;
    public GameObject[] loadPanel;
    //tham chieu
    private PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    void Start()
    {
        mapUIBG.SetActive(false);
        mapUIConten.SetActive(false);
        isTurnOffMap =false;
        mapAudio = GetComponent<AudioSource>();

    // Ẩn chuột lúc bắt đầu nếu cần
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();


    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && playerStatus.currentHp > 0 && panelSkillTree.activeSelf == false 
        && imageBGButtonInven.activeSelf == false && EquipMentMenu.activeSelf == false && isOpenMap && playerControllerState.animator.enabled)
        {
            if (mapUIConten.activeSelf)
            {
                isTurnOffMap = false;
                mapAudio.PlayOneShot(mapClip);
                mapUIConten.SetActive(false);
                mapUIBG.SetActive(false);
                Time.timeScale = 1f;
                TurnOffOnUI.pause = false;
                // Ẩn chuột khi tắt map
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isTurnOffMap = true;
                mapAudio.PlayOneShot(mapClip);
                mapUIConten.SetActive(true);
                mapUIBG.SetActive(true);
                TurnOffOnUI.pause = true;
                Time.timeScale = 0f;
                // Hiện chuột khi mở map
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
       
        if(Input.GetKeyDown(KeyCode.M)){
             scrollMap.verticalNormalizedPosition = scrollMapVertical; // Top
             scrollMap.horizontalNormalizedPosition = scrollMapHorizontal; // Left (nếu dùng cuộn ngang)
        }
        if (!mapUIBG.activeSelf)
        {
            isTurnOffMap = false; // Đặt lại biến khi map không hiển thị
        }
    }
}
