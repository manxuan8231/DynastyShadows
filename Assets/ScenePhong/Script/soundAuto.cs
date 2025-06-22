using UnityEngine;

public class AutoEnvironmentMusic : MonoBehaviour
{
    private AudioSource myAudio;
    private bool wasPausedByOtherMusic = false;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        if (myAudio == null)
        {
            Debug.LogError("Không tìm thấy AudioSource trên GameObject này.");
        }
    }

    void Update()
    {
        if (myAudio == null) return;

        bool otherMusicPlaying = false;

        // Duyệt tất cả AudioSource trong scene
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            // Bỏ qua bản thân
            if (audio == myAudio) continue;

            // Nếu AudioSource khác đang phát và là nhạc (loop hoặc clip dài)
            if (audio.isPlaying && audio.clip != null && audio.clip.length > 10f)
            {
                otherMusicPlaying = true;
                break;
            }
        }

        // Nếu có nhạc khác -> tạm dừng nhạc môi trường
        if (otherMusicPlaying)
        {
            if (myAudio.isPlaying)
            {
                myAudio.Pause();
                wasPausedByOtherMusic = true;
            }
        }
        else
        {
            // Nếu không còn nhạc khác và trước đó bị tạm dừng -> bật lại
            if (!myAudio.isPlaying && wasPausedByOtherMusic)
            {
                myAudio.UnPause();
                wasPausedByOtherMusic = false;
            }
        }
    }
}
