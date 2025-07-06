using UnityEngine;

public class FireTargetPL : MonoBehaviour
{
    public GameObject[] firePrefab;
    public float speed = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Xoay vòng quanh 
        foreach (GameObject fire in firePrefab)
        {
            if (fire != null)
            {
                fire.transform.RotateAround(transform.position, -Vector3.right, speed * Time.deltaTime);
            }
        }
    }
}
