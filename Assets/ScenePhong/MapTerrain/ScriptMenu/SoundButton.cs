using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio; // Quan trọng để sử dụng AudioMixerGroup

[RequireComponent(typeof(AudioSource))]
public class UIButtonSoundManager : MonoBehaviour
{
    [Header("Âm thanh mặc định")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Âm thanh riêng cho nút Play")]
    public AudioClip playHoverSound;
    public AudioClip playClickSound;

    [Header("Audio Mixer Group")]
    public AudioMixerGroup mixerGroup; // <-- thêm Mixer Group

    void Start()
    {
        // Tìm tất cả Button trong GameObject con
        Button[] buttons = GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            string btnName = btn.gameObject.name;

            // Thêm AudioSource nếu chưa có
            AudioSource source = btn.gameObject.GetComponent<AudioSource>();
            if (source == null)
                source = btn.gameObject.AddComponent<AudioSource>();

            // Gán mixer group (nếu có)
            if (mixerGroup != null)
                source.outputAudioMixerGroup = mixerGroup;

            // Tạo handler và gán âm tương ứng
            UIButtonSoundHandler handler = btn.gameObject.AddComponent<UIButtonSoundHandler>();

            if (btnName == "Play")
            {
                handler.Setup(source, playHoverSound, playClickSound);
            }
            else
            {
                handler.Setup(source, hoverSound, clickSound);
            }
        }
    }

    private class UIButtonSoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        private AudioSource audioSource;
        private AudioClip hoverSound;
        private AudioClip clickSound;

        public void Setup(AudioSource source, AudioClip hover, AudioClip click)
        {
            audioSource = source;
            hoverSound = hover;
            clickSound = click;
            audioSource.playOnAwake = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverSound != null)
                audioSource.PlayOneShot(hoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (clickSound != null)
                audioSource.PlayOneShot(clickSound);
        }
    }

}
