using System.IO;
using UnityEngine;

public class SavePositionPL : MonoBehaviour
{
    public GameObject player; // Tham chiếu đến player
    private string savePath;

    // Dữ liệu vị trí player
    [System.Serializable]
    public class PlayerPosition
    {
        public float x;
        public float y;
        public float z;
    }

    private void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player"); // Tự động tìm nếu chưa gán
        savePath = Application.persistentDataPath + "/player_position.json";// đường dẫn file lưu vị trí player

        //LoadPosition(); // Load vị trí khi vào game
    }

    private void OnApplicationQuit()
    {
        SavePosition(); // Lưu vị trí khi thoát game
    }

    public void SavePosition()
    {
        Vector3 pos = player.transform.position;
        PlayerPosition data = new PlayerPosition
        {
            x = pos.x,
            y = pos.y,
            z = pos.z
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Đã lưu vị trí vào JSON: " + savePath);
    }

    public void LoadPosition()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerPosition data = JsonUtility.FromJson<PlayerPosition>(json);

            Vector3 loadedPos = new Vector3(data.x, data.y, data.z);
            player.transform.position = loadedPos;

        }

    }
}