using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject[] skillTreeUI;
    public PlayerControllerState playerControllerState;
    void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
    }

   
    void Update()
    {
        //ẩn UI skill tree khi animator bị tắt
        if (playerControllerState.animator.enabled == false)
        {
            foreach (GameObject ui in skillTreeUI)
            {
                ui.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject ui in skillTreeUI)
            {
                ui.SetActive(true);
            }
        }
    }
}
