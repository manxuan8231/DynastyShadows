using UnityEngine;

public class Skill4Core : MonoBehaviour
{
    public GameObject[] shield;
    public float shieldDuration = 5f;


    
    void Update()
    {
        foreach (GameObject s in shield)
        {
          s.transform.RotateAround(transform.position, Vector3.up, shieldDuration * Time.deltaTime);
        }
    }
}
