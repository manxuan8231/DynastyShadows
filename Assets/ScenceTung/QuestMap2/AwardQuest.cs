using UnityEngine;

public class AwardQuest : MonoBehaviour
{
    PlayerStatus playerStatus;
    AudioCanvasState audioCanvasState;
    public GameObject canvasAward;
    void Start()
    {
     playerStatus = FindAnyObjectByType<PlayerStatus>();
     audioCanvasState = FindAnyObjectByType<AudioCanvasState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
