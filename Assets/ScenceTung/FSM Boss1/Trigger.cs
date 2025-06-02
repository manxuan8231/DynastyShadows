using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject modelPlayer;
    public GameObject modelBoss;
    public GameObject effect;
   public AnmtToNam anmtTn;
    public Transform player;
    Vector3 playerPos;
    public CinemachineCamera cam1;
    public CinemachineCamera camPlayer;
    public CinemachineCamera cam3;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        anmtTn = FindAnyObjectByType<AnmtToNam>();  
        effect.SetActive(false);
        modelBoss.SetActive(false);
        
        
    }

    // Update is called once per frame
    void Update()
    {
       player.transform.position = playerPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(ChangModel());
        }
    }
    IEnumerator ChangModel()
    {
        anmtTn.Changed();
        yield return new WaitForSeconds(6f);
        effect.SetActive(true);
        modelPlayer.SetActive(false); 
        yield return new WaitForSeconds(4.5f);
        effect.SetActive(false);
        yield return null;
        modelBoss?.SetActive(true);
        
    }
}
