using UnityEngine;

public class EvenAlien : MonoBehaviour
{
   public CameraShake cameraShake;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
    }

    
    void Update()
    {
        
    }
    public void ShakeCamera()
    {
        cameraShake.Shake();
    }
}
