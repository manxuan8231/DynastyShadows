using UnityEngine;

public class ParryTrigger : MonoBehaviour
{
    public bool isParry = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
