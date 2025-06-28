using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill4Shield : MonoBehaviour
{
    public GameObject[] shield;
    public float shieldDuration = 5f;

    public GameObject player;
    //tham chieu
    private PlayerControllerState playerControllerState;
    private PlayerStatus playerStatus;
    private void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
       
        if (playerControllerState == null)
            playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        if (playerStatus == null)
            playerStatus = FindAnyObjectByType<PlayerStatus>();

        StartCoroutine(WaitOff());
    }
    void Update()
    {
        //xoay shield
        foreach (GameObject s in shield)
        {
          s.transform.RotateAround(transform.position, Vector3.up, shieldDuration * Time.deltaTime);
        }   
        if(player != null)
        {
          
            transform.position = player.transform.position + new Vector3(0,1f,0);        
        }
        if(playerStatus.shieldHealth <= 0)
        {
            playerStatus.isShieldActive = false;
            playerStatus.shieldHealthObject.SetActive(false);
            Destroy(gameObject);
        }
    }
   public IEnumerator WaitOff()
    {
        playerStatus.shieldHealthObject.SetActive(true);
        playerStatus.shieldHealth = playerStatus.shieldMaxHealth;
        playerStatus.healthShielSlider.value = playerStatus.shieldHealth;
        playerStatus.isShieldActive = true;
        yield return new WaitForSeconds(7f);
       
        playerStatus.isShieldActive = false;
        playerStatus.shieldHealthObject.SetActive(false);
        Destroy(gameObject);
    }
   
}
