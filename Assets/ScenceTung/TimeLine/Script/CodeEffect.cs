using System.Collections;
using UnityEngine;

public class CodeEffect : MonoBehaviour
{
    public GameObject[] shield;
    public float shieldDuration = 5f;

  
 
    void Update()
    {
        //xoay shield
        foreach (GameObject s in shield)
        {
            s.transform.RotateAround(transform.position, Vector3.up, shieldDuration * Time.deltaTime);
        }
      
       
    }
   
  
}
