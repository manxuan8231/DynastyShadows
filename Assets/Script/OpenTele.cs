using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;       // Panel Teleport
    public GameObject buttonF;       // UI nhấn F
    public string teleportID = "teleport_1"; // ID riêng cho mỗi teleport

    private bool isInRange = false;
    public Collider colliderOpen;
    public AudioSource audioSource;
    public AudioClip soundF;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "teleport_data.json");
    private static TeleportSaveData saveData;

    void Start()
    {
        imgTele.SetActive(false);
        buttonF.SetActive(false);
        colliderOpen.enabled = true;
        audioSource = GetComponent<AudioSource>();

        LoadTeleportData();

        if (saveData.unlockedTeleports.Contains(teleportID))
        {
            imgTele.SetActive(true);
            colliderOpen.enabled = false;
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            isInRange = false;
            audioSource.PlayOneShot(soundF);
            buttonF.SetActive(false);
            colliderOpen.enabled = false;
            imgTele.SetActive(true);

            if (!saveData.unlockedTeleports.Contains(teleportID))
            {
                saveData.unlockedTeleports.Add(teleportID);
                SaveTeleportData();
            }
        }

        // Reset toàn bộ teleport bằng Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            File.Delete(SavePath);
            saveData = new TeleportSaveData();
            Debug.Log("Đã xóa dữ liệu teleport.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(true);
            isInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(false);
            isInRange = false;
        }
    }

   

    private void LoadTeleportData()
    {
        if (saveData != null) return;

        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            saveData = JsonUtility.FromJson<TeleportSaveData>(json);
        }
        else
        {
            saveData = new TeleportSaveData();
        }
    }

    private void SaveTeleportData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }



    [System.Serializable]
    public class TeleportSaveData
    {
    public List<string> unlockedTeleports = new List<string>();
    }

}
