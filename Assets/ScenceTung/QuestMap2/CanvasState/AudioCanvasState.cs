using UnityEngine;

public class AudioCanvasState : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip doneQuest;
    public AudioClip newQuest;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayDoneQuest()
    {
        audioSource.clip = doneQuest;
        audioSource.Play();
    }
    public void PlayNewQuest()
    {
        audioSource.clip = newQuest;
        audioSource.Play();
    }
}
