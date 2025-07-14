using UnityEngine;

public class DashAfterImgAssa : MonoBehaviour
{
    public float fadeDuration = 0.5f; // mo dan
    private MeshRenderer[] renderers;
    private Material[] materials;
    private float startTime;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
        startTime = Time.time;
    }
    private void Update()
    {
        float t = (Time.time - startTime) / fadeDuration;
        Color color;

        for (int i = 0; i < materials.Length; i++)
        {
            color = materials[i].color;
            color.a = Mathf.Lerp(1f, 0f, t);
            materials[i].color = color;
           
        }
    }
}
