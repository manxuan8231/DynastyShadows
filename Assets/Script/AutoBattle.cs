using UnityEngine;

public class AutoBattle : MonoBehaviour
{
    public Coroutine autoBattleCoroutine;
    //tham chieu
    public PlayerStatus playerStatus;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
    }

    void Update()
    {
         
    }
}
