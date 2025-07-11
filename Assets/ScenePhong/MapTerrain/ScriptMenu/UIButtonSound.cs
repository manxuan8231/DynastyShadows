using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Âm riêng cho nút (tuỳ chọn)")]
    public AudioClip hoverSoundOverride;
    public AudioClip clickSoundOverride;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIAudioManagerExists())
            UIAudioManagerInstance().PlayHover(hoverSoundOverride);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIAudioManagerExists())
            UIAudioManagerInstance().PlayClick(clickSoundOverride);
    }

    private bool UIAudioManagerExists() => UIAudioManager.Instance != null;
    private UIAudioManager UIAudioManagerInstance() => UIAudioManager.Instance;
}
