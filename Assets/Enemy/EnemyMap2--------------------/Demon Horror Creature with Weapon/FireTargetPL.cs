using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTargetPL : MonoBehaviour
{
    public GameObject[] firePrefab;
    public float fireDuration = 100f; // Tốc độ xoay
    public float fireRate = 1f; // Thời gian giữa các lần bắn
    public GameObject fireBallBulletPrefab;
    public Transform player;

    private List<GameObject> fireList = new List<GameObject>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (GameObject fire in firePrefab)
        {
            fireList.Add(fire);
        }

        StartCoroutine(FireSequentially());
        StartCoroutine(DestroyGameObject());
    }

    private void Update()
    {
        // Xoay vòng quanh 
        foreach (GameObject fire in fireList)
        {
            if (fire != null)
            {
                fire.transform.RotateAround(transform.position, Vector3.right, fireDuration * Time.deltaTime);
            }
        }

        // Tìm object tên là "PosiShoter" gần nhất
        GameObject[] allObjs = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in allObjs)
        {
            if (obj.name == "Ball-root")
            {
                float dist = Vector3.Distance(transform.position, obj.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = obj;
                }
            }
        }

        if (closest != null)
        {
            // Gán vị trí hiện tại bằng với vị trí "PosiShoter" gần nhất
            transform.position = closest.transform.position;
        }
    }


    IEnumerator FireSequentially()
    {
        while (fireList.Count > 0)
        {
            GameObject fire = fireList[0];
            fireList.RemoveAt(0);

            if (fire != null)
            {
                Vector3 shootDir = (player.position - fire.transform.position).normalized;

                // Tạo viên đạn mới tại vị trí fire đang xoay
                GameObject bullet = Instantiate(fireBallBulletPrefab, fire.transform.position, Quaternion.identity);
                bullet.transform.forward = shootDir;
                Destroy(fire);

                yield return new WaitForSeconds(fireRate);
            }
        }
    }
    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(10f); // Thời gian tồn tại của viên đạn
        Destroy(gameObject); // Xoá viên đạn sau 5 giây
    }
}
