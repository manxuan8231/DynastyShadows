
using UnityEngine;

public class EvenAnimatorAssa : MonoBehaviour
{
  
        //tham chieu
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
}
