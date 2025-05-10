using UnityEngine;
using UnityEngine.UI;

public class AnimationAvatar : MonoBehaviour
{
   
    public RawImage rawImage;   
    public Texture2D[] images = new Texture2D[6];  
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

    public void TakeValueAvatar(int amount) // trừ value avatar
    {
        currentValue -= amount;
        currentValue = Mathf.Clamp(currentValue, 1, images.Length);
        rawImage.texture = images[currentValue - 1];
    }
    public void AddValueAvatar(int amount) // cộng value avatar
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 1, images.Length);
        rawImage.texture = images[currentValue - 1];
    }
}
