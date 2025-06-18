using System.Collections;
using UnityEngine;

public class Skill2ClonePL : MonoBehaviour
{
    public GameObject effectExplotion;
    public Skill2Manager skill2Manager;
    void Start()
    {
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        //đợi để khỏi tạo hiệu ứng nổ
        if(skill2Manager.isExplosionSkill2 == true)
            StartCoroutine(WaitEffect(skill2Manager.timeSkill2));
    }

   
    void Update()
    {
        //click chuột để tạo hiệu ứng nổ
        if (Input.GetMouseButtonDown(0) && skill2Manager.isExplosionSkill2 == true && Cursor.visible == false)
        {
            transform.position = new Vector3(0f, 6f, 0f);
            GameObject instan = Instantiate(effectExplotion, transform.position, Quaternion.identity);
            Destroy(instan, 2f);
        }
    }
    public IEnumerator WaitEffect(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        transform.position = new Vector3(0f, 6f, 0f);
        GameObject instan = Instantiate(effectExplotion, transform.position, Quaternion.identity);
        Destroy(instan, 2f);
    }
}
