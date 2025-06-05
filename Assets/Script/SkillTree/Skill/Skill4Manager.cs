using UnityEngine;

public class Skill4Manager : MonoBehaviour
{
    public SkinnedMeshRenderer[] skill4MeshRenderers; // Mảng chứa các SkinnedMeshRenderer của skill 4
    public GameObject effectAura; // Hiệu ứng Aura của skill 4
    public bool isInputSkill4 = false;//kiem tra de chuyen trang thai skill4


    void Start()
    {
        isInputSkill4 = false; // Khởi tạo trạng thái skill 4 là không được kích hoạt
        effectAura.SetActive(false); // Tắt hiệu ứng Aura ban đầu
        ToggleSkill4(false); // Tắt skill 4 ban đầu
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) 
        {
            isInputSkill4 = true;
            ToggleSkill4(true); // Bật skill 4 khi nhấn phím 4   
        }
    }
    public void ToggleSkill4(bool isActive)
    {
        foreach (var renderer in skill4MeshRenderers)
        {
            renderer.enabled = isActive; // Bật hoặc tắt tất cả các SkinnedMeshRenderer
        }
    }
}
