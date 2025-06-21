using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class TakeQuest1 : MonoBehaviour
{
    public CinemachineCamera cam1;
    public GameObject canvasText;
    public TMP_Text content;
    public PlayerControllerState playerControllerState;
    public GameObject trigger;
    public CinemachineBrain cinemachineBrain;
    private void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Camera());
        }
    }

    IEnumerator Camera()
    {
        playerControllerState.enabled = false;
        playerControllerState.animator.enabled = false;
        canvasText.SetActive(true);
        cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, 0.2f); // Fixed CS0120 and CS0176
        content.text = "Hình như đây là quyển nhất ký của binh lính ấy?";
        cam1.Priority = 20;
        yield return new WaitForSeconds(3f);
        content.text = "Hãy mở nó ra xem nào!";
        yield return new WaitForSeconds(5f);
        playerControllerState.enabled = true;
        playerControllerState.animator.enabled = true;
        cam1.Priority = 0;
        cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 2f);
        canvasText.SetActive(false);

        Destroy(trigger);
    }
}
