using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

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
    public AudioMixerGroup mixerGroup;

    private AudioSource sharedAudioSource;

    void Start()
    {
        // Dùng AudioSource dùng chung
        sharedAudioSource = GetComponent<AudioSource>();
        sharedAudioSource.playOnAwake = false;
        if (mixerGroup != null)
            sharedAudioSource.outputAudioMixerGroup = mixerGroup;

        // Gắn âm thanh cho mỗi nút
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            var handler = btn.gameObject.AddComponent<UIButtonSoundHandler>();
            string btnName = btn.gameObject.name;

            if (btnName == "Play")
                handler.Setup(sharedAudioSource, playHoverSound, playClickSound);
            else
                handler.Setup(sharedAudioSource, hoverSound, clickSound);
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
