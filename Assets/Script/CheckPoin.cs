using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController player = other.GetComponent<CharacterController>();
            if (player != null)
            {
                Debug.Log("Da luu diem checkpoint!");
                CheckpointHandler.SaveCheckpoint(player.transform.position);

                GameSaveData data = SaveManagerMan.LoadGame();
             

                data.savedSceneName = SceneManager.GetActiveScene().name;



                // ✅ Cuối cùng, lưu lại
                SaveManagerMan.SaveGame(data);
            }
        }
    }
}
