using UnityEngine;
using UnityEngine.UI;

public class AnimationAvatar : MonoBehaviour
{
    [Header("Tham chiếu")]
    public RawImage rawImage;

    [Header("Danh sách ảnh (tối đa 6)")]
    public Texture2D[] images = new Texture2D[6];

    [Header("Giá trị (1 - 6)")]
    [Range(1, 6)]
    public int currentValue = 1;

    void Update()
    {
        // Đảm bảo currentValue nằm trong khoảng hợp lệ
        int index = Mathf.Clamp(currentValue - 1, 0, images.Length - 1);

        // Kiểm tra nếu ảnh tồn tại thì hiển thị
        if (images[index] != null)
        {
            rawImage.texture = images[index];
        }
    }
}
