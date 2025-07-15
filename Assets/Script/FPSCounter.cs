using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private float updateInterval = 0.5f;
    private float timer = 0f;
    private int frameCount = 0;

    void Update()
    {
        frameCount++;
        timer += Time.unscaledDeltaTime;

        if (timer >= updateInterval)
        {
            float fps = frameCount / timer;
            fpsText.text = $"FPS: {fps:0}";
            timer = 0f;
            frameCount = 0;
        }
    }
}
