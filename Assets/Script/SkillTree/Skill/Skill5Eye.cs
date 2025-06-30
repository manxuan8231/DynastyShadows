using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill5Eye : MonoBehaviour
{
    public GameObject panelDisplay;
    public Material material; // Material mới để áp dụng
    public string targetLayerName = "Interactable"; // Layer đối tượng highlight
    public float searchRadius = 30f;

    private GameObject currentTarget;
    private Material originalMaterial; // Material gốc
    public bool isInputSkill; // Cờ bật kỹ năng

    // Biến làm chậm enemy
    private Animator slowedEnemyAnimator;
    private float originalAnimSpeed;
    private List<Animator> slowedAnimators = new List<Animator>();

    void Start()
    {
        panelDisplay.SetActive(false);
    }

    void Update()
    {
        if (isInputSkill)
        {
            StartCoroutine(WaitOffSkill());
        }
    }

    public IEnumerator WaitOffSkill()
    {
        StartMaterial(); // Bật kỹ năng
        isInputSkill = false;

        yield return new WaitForSeconds(7f);

        EndMaterial(); // Kết thúc kỹ năng
    }

    public void StartMaterial()
    {
        panelDisplay.SetActive(true);

        FindObject();        // Tìm và highlight object
        SlowEnemiesInArea();
    }

    public void EndMaterial()
    {
        panelDisplay.SetActive(false);

        // Trả lại material ban đầu
        if (currentTarget != null && originalMaterial != null)
        {
            Renderer rend = currentTarget.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = originalMaterial;
            }

            currentTarget = null;
            originalMaterial = null;
        }

        // Trả lại tốc độ Animator 
        foreach (Animator anim in slowedAnimators)
        {
            if (anim != null)
            {
                anim.speed = 1f;
            }
        }
        slowedAnimators.Clear();

    }

    public void FindObject()
    {
        int layerMask = LayerMask.GetMask(targetLayerName);
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, layerMask);

        float closestDistance = Mathf.Infinity;
        GameObject closestObject = null;

        foreach (Collider hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestObject = hit.gameObject;
            }
        }

        if (closestObject != null)
        {
            currentTarget = closestObject;

            Renderer rend = currentTarget.GetComponent<Renderer>();
            if (rend != null)
            {
                originalMaterial = rend.material;
                rend.material = material;
            }
        }
    }

    public void SlowEnemiesInArea()
    {
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] enemies = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);

        slowedAnimators.Clear(); // Xóa danh sách cũ

        foreach (Collider enemy in enemies)
        {
            Animator anim = enemy.GetComponent<Animator>();
            if (anim != null)
            {
                slowedAnimators.Add(anim);
                anim.speed = 0.2f; // Làm chậm
            }
        }
    }

}
