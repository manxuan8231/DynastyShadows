using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 2f;
    public float shakeFrequency = 2f;

    private CinemachineCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer = 0f;

    void Awake()
    {
        cinemachineCam = GetComponent<CinemachineCamera>();
        noise = cinemachineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f && noise != null)
            {
                noise.AmplitudeGain = 0f;
                noise.FrequencyGain = 0f;
            }
        }
       
    }

    public void Shake()
    {
        if (noise == null) return;

        shakeTimer = shakeDuration;
        noise.AmplitudeGain = shakeAmplitude;
        noise.FrequencyGain = shakeFrequency;
    }
}
