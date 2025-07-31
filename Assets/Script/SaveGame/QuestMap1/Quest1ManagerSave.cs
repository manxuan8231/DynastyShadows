using UnityEngine;

public class Quest1ManagerSave : MonoBehaviour
{
    public GameObject quest1;//bai go bac lam
    public GameObject quest2;//dieu tra khu danh ca
    public GameObject quest3;//linh canh a
    public GameObject questShop;//cua hang cua thuong nhan
    public GameObject questBacLamAndLinhB;//bac lam va linh canh b quest  
    public GameObject questDesert5;//quest sa mac map 1
    public GameObject boss;//quest boss map 1
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
        //kiem tra neu quest3 hoan thanh thi tat bat questbaclam and linh b
        if (dataQuest.isQuest3Map1)
        {
            quest3.SetActive(false);
            questBacLamAndLinhB.SetActive(true);
            questDesert5.SetActive(true);
        }
        else
        {
            quest3.SetActive(true);
        }
        //kiem tra hoan thanh nhiem vu sa mac bat quest boss
      
        if (dataQuest.isQuestBossMap1)
        {
          
            questBacLamAndLinhB.SetActive(false);
            questDesert5.SetActive(false);
            boss.SetActive(true);
           
        }
        else
        {
            boss.SetActive(false);
        }
    }


}
