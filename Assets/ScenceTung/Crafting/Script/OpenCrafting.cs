using UnityEngine;

public class OpenCrafting : MonoBehaviour
{
    public GameObject craftingUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            craftingUI.SetActive(!craftingUI.activeSelf);
        }
    }
}
