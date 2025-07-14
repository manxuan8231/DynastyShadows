using UnityEngine;

public class ZoneMusicTrigger : MonoBehaviour
{
    [Tooltip("Tên khu vực, dùng để lấy nhạc tương ứng từ AudioManager")]
    public string zoneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManagerPMarket.Instance.PlayZoneMusic(zoneName);
        }
    }
}
