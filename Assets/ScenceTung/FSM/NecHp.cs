using UnityEngine;

public class NecHp : MonoBehaviour
{
    public float curhp;
    public float maxhp = 500f;
    void Start()
    {
        curhp = maxhp;
    }


    public void TakeDame(float damage)
    {
        curhp -= damage;   
        curhp = Mathf.Clamp(curhp, 0, maxhp);
    }
}
