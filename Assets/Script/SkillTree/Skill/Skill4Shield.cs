using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill4Shield : MonoBehaviour
{
    public GameObject[] shield;
    public float shieldDuration = 5f;
    public GameObject player;
    public GameObject effectBreak;

    public Transform shield1;
    public Transform shield2;
    public Transform shield3;
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
          
            transform.position = player.transform.position + new Vector3(0,1.5f,0);        
        }
        if(playerStatus.shieldHealth <= 0)
        {
            playerStatus.isShieldActive = false;
            playerStatus.shieldHealthObject.SetActive(false);
            EffectBreak();
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
        EffectBreak();
        Destroy(gameObject);
   }

    public void EffectBreak()
    {
        GameObject e1 = Instantiate(effectBreak, shield1.position + Vector3.up * 0.5f, Quaternion.identity);
        GameObject e2 = Instantiate(effectBreak, shield2.position + Vector3.up * 0.5f, Quaternion.identity);
        GameObject e3 = Instantiate(effectBreak, shield3.position + Vector3.up * 0.5f, Quaternion.identity);

        Destroy(e1, 1f);
        Destroy(e2, 1f);
        Destroy(e3, 1f);
    }

}
