
using UnityEngine;

public class EvenAnimatorAssa : MonoBehaviour
{
    //ao anh
    public GameObject shadowPrefab; 

    [Header("Tham Chieu")]
    public DameZoneLeftAssa dameZoneLeftAssa;
    public DameZoneRightAssa dameZoneRightAssa;
    void Start()
    {
        dameZoneLeftAssa = FindAnyObjectByType<DameZoneLeftAssa>();
        dameZoneRightAssa = FindAnyObjectByType <DameZoneRightAssa>();

       
    }

    
    void Update()
    {
        
    }
    //tay trai
    public void BeginDameLeft()
    {
        dameZoneLeftAssa.BeginDame();
    }
    public void EndDameLeft() 
    {
        dameZoneLeftAssa.EndDame();
    }
    //tay phai
    public void BeginDameRight()
    {
        dameZoneRightAssa.BeginDame();
    }
    public void EndDameRight()
    {
        dameZoneRightAssa.EndDame();
    }
    public void BeginDameBack()//hit back
    {
        dameZoneRightAssa.BeginDameBack();
    }
    public void EndDameBack()
    {
        dameZoneRightAssa.EndDameBack();
    }

    //tao bong
    public void StartShadow()
    {
        GameObject instan = Instantiate(shadowPrefab,transform.position,transform.rotation);
        
    }
}
