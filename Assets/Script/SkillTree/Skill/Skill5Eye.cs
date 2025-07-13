using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill5Eye : MonoBehaviour
{
    public GameObject panelDisplay;  
    public string targetLayerName = "Interactable"; // Layer đối tượng highlight
    public GameObject iconPrefab; // Prefab UI
    public Canvas uiCanvas;       // Gắn canvas chính
    public float searchRadius = 30f;
    public bool isInputSkill; // Cờ bật kỹ năng

    // Biến làm chậm enemy
    private List<Animator> slowedAnimators = new List<Animator>();
    private List<GameObject> spawnedIcons = new List<GameObject>();
    private List<Transform> trackedTargets = new List<Transform>();
    void Start()
    {
        panelDisplay.SetActive(false);
    }

    void Update()
    {
        if (isInputSkill)
        {
            StartCoroutine(WaitOffSkill());
            isInputSkill = false;
        }

        // Cập nhật vị trí icon theo object
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            if (spawnedIcons[i] != null && trackedTargets[i] != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(trackedTargets[i].position + Vector3.up * 2.2f);

                // Ẩn icon nếu đối tượng ra sau lưng camera
                if (screenPos.z > 0)
                {
                    spawnedIcons[i].SetActive(true);
                    spawnedIcons[i].transform.position = screenPos;
                }
                else
                {
                    spawnedIcons[i].SetActive(false); // Tắt icon nếu ở sau camera
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            isInputSkill = true;
        }
    }


    public IEnumerator WaitOffSkill()
    {
        StartMaterial(); // Bật kỹ năng
        isInputSkill = false;
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("InvisibleAssasin")); // Hiện lại layer mặc định
        
        yield return new WaitForSeconds(6f);
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("InvisibleAssasin")); // Ẩn layer mặc định
        EndMaterial(); // Kết thúc kỹ năng
    }

    public void StartMaterial()
    {
        panelDisplay.SetActive(true);

        FindObject();        // Tìm và highlight object
    }

    public void EndMaterial()
    {
        panelDisplay.SetActive(false);


        // Trả lại tốc độ Animator 
        foreach (Animator anim in slowedAnimators)
        {
            if (anim != null)
            {
                anim.speed = 1f;
            }
        }
        foreach (GameObject icon in spawnedIcons)
        {
            if (icon != null) Destroy(icon);
        }
        spawnedIcons.Clear();
        trackedTargets.Clear();
        slowedAnimators.Clear();

    }

    public void FindObject()
    {
        //int layerMask = LayerMask.GetMask(targetLayerName);
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                GameObject obj = hit.gameObject;
                Transform target = obj.transform;

                GameObject icon = Instantiate(iconPrefab, uiCanvas.transform);
                spawnedIcons.Add(icon);
                trackedTargets.Add(target);
            }
            
        }
    }


   

}
