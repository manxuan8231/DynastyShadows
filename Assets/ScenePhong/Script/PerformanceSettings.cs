using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Tắt VSync
        Application.targetFrameRate = 120; // Hoặc bạn có thể đặt thành 300, tùy game
    }
}
