using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Tắt VSync
    }
}
