
using UnityEngine;

public class EvenAnimatorAssa : MonoBehaviour
{
    //ao anh
    public GameObject shadowPrefab;
    //phong dao
    public GameObject knifePrefab;
    public Transform knifeSpawnPoint;
    [Header("Tham Chieu")]
    public DameZoneLeftAssa dameZoneLeftAssa;
    public DameZoneRightAssa dameZoneRightAssa;
    public ControllerStateAssa controllerStateAssa;
    void Start()
    {
        dameZoneLeftAssa = FindAnyObjectByType<DameZoneLeftAssa>();
        dameZoneRightAssa = FindAnyObjectByType <DameZoneRightAssa>();
        controllerStateAssa = FindAnyObjectByType<ControllerStateAssa>();


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
    //phong dao 
    public void KnifeThrower()
    {
        // Tạo dao tại vị trí spawn
        GameObject instan = Instantiate(knifePrefab, knifeSpawnPoint.position, Quaternion.identity);

        Rigidbody rb = instan.GetComponent<Rigidbody>();
        if (rb != null && controllerStateAssa.player != null)
        {
            // Hướng từ enemy tới player
            Vector3 direction = (controllerStateAssa.player.transform.position - knifeSpawnPoint.position).normalized;
            
            Quaternion lookRot = Quaternion.LookRotation(direction); // hướng về player
            lookRot *= Quaternion.Euler(-90f, 0f, 0f); 
            instan.transform.rotation = lookRot;
            // Bắn dao theo hướng đó
            rb.AddForce(direction * 50f, ForceMode.Impulse);

           
        }
    }

}
