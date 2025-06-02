using UnityEngine;

public class AnmtToNam : MonoBehaviour
{
    public Animator anmt;    
    public Trigger trigger;
    void Start()
    {
        trigger = FindAnyObjectByType<Trigger>();
        anmt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Changed()
    {
        anmt.SetTrigger("ChangeModel");
        trigger.transform.LookAt(trigger.player);
    }
}
