using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionPL : MonoBehaviour
{
    public GameObject player; // Tham chiếu đến đối tượng người chơi
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // Tìm đối tượng người chơi nếu chưa gán
        }
        LoadPosition(); // Khi game bắt đầu thì load vị trí nếu có
    }

    private void OnApplicationQuit()
    {
        SavePosition(); // Khi thoát game thì lưu lại vị trí hiện tại
    }

    public void SavePosition()
    {
        Vector3 pos = player.transform.position;

        PlayerPrefs.SetFloat("PlayerX", pos.x);
        PlayerPrefs.SetFloat("PlayerY", pos.y);
        PlayerPrefs.SetFloat("PlayerZ", pos.z);

        PlayerPrefs.Save(); // Lưu lại 
    }

    public void LoadPosition()
    {
       
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            Vector3 loadedPos = new Vector3(x, y, z);
            player.transform.position = loadedPos;
           
        }
    }

}
