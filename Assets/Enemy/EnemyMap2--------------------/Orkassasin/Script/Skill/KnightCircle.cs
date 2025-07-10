using UnityEngine;

public class KnightCircle : MonoBehaviour
{
    public GameObject knife;//xoay chinh ban than
    public float speed;

    public GameObject[] knifePre;//xoay cac vat the xung quanh
    void Start()
    {
        
    }

    
    void Update()
    {
        if (knife != null)
        {
            knife.transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
      

        foreach (var knife in knifePre)
        {
            knife.transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
        }
    }
}
