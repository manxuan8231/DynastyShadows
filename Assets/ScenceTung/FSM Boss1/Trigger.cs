using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using TMPro;

public class Trigger : MonoBehaviour
{
    public GameObject modelPlayer;
    public GameObject modelBoss;
    public GameObject effect;
    public AnmtToNam anmtTn;
    public Transform player;
    public Vector3 playerPos;
    public GameObject canvasText;
    
    public CinemachineCamera cam1;
    public TMP_Text textContent;
    bool isChangedModel = false;
    public BoxCollider boxTrigger;
    public GameObject sliderBoss;
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
        playerPos = player.transform.position;
        sliderBoss.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isChangedModel)
        {
            StartCoroutine(ChangModel());
            isChangedModel = true;
        }
    }
    IEnumerator ChangModel()
    {
        boxTrigger.enabled = false;
        canvasText.SetActive(true);
        CharacterController characterController = FindAnyObjectByType<CharacterController>();
        characterController.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        textContent.text = "Là cậu ư?....";
        cam1.Priority = 99;
        yield return new WaitForSeconds(2.8f);
        textContent.text = "Cậu đã ở đâu trong suốt 1 tháng qua?...";
        yield return new WaitForSeconds(5);
        canvasText.SetActive(false);
        anmtTn.Changed();
    
        yield return new WaitForSeconds(6f);
        effect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        modelPlayer.SetActive(false); 
        yield return new WaitForSeconds(4.5f);
        effect.SetActive(false);
        yield return new WaitForSeconds(0.3f) ;
        modelBoss.SetActive(true);
        yield return new WaitForSeconds(2);
        sliderBoss.SetActive(true);
        cam1.Priority = 0;
        characterController.enabled = true;
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
        
    }
}
