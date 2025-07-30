using UnityEngine;

public class Quest1ManagerSave : MonoBehaviour
{
    public GameObject quest1;
    public GameObject quest2;
    public GameObject quest3;
    public GameObject questShop;
    void Start()
    {
        DataQuest dataQuest = SaveManagerMan.LoadGame().dataQuest;
        // nếu quest1 bằng true thì tắt quest1 bật quest2
        if (dataQuest.isQuest1Map1 == true)
        {
            quest1.SetActive(false);
            quest2.SetActive(true);
        }
        else
        {
            quest1.SetActive(true);
        }
        // kiểm tra nếu quest2 bằng true thì tắt quest2 bật quest3 và questshop
        if (dataQuest.isQuest2Map1 == true)
        {
            quest2.SetActive(false);
            quest3.SetActive(true);
            questShop.SetActive(true);
        }
        else
        {
            quest2.SetActive(true);
        }

    }

    void Update()
    {
        
    }
}
