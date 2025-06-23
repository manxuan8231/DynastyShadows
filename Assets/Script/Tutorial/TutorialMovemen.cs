using UnityEngine;

public class TutorialMovemen : MonoBehaviour
{
    public enum TutorialState
    {
        None,
        Walk,
        Run,
    }
    public TutorialState currentState = TutorialState.None;

    public bool isTriggered = false; // Biến để kiểm soát trạng thái của collider
   

    //tham chieuy
    public TutorialManager tutorialManager;
    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
    }

   
    void Update()
    {
        if(isTriggered == true && currentState == TutorialState.Walk)
        {
            tutorialManager. isTutorialWalk = true; 
        }
        if (isTriggered == true && currentState == TutorialState.Run)
        {
            tutorialManager. isTutorialRun = true; 
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true; 
            Destroy(gameObject,0.5f); 
        }
    }
}
