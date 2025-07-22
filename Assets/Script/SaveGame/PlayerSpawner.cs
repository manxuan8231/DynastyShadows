using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private static GameObject currentPlayer;

    void Awake()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
