using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    public GameObject player;
    public float positionY = 30f;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, positionY, player.transform.position.z);
    }
}
