using UnityEngine;
using System.Collections;

public class LoadSceneRandom : MonoBehaviour
{
    public GameObject[] loadingImages; // Các hình ảnh loading
    private GameObject currentImage;

    void OnEnable()
    {
        // Bắt đầu coroutine random ảnh
        StartCoroutine(RandomImageRoutine());
    }

    IEnumerator RandomImageRoutine()
    {
        // Ẩn tất cả ảnh
        HideAllImages();

        // Random ảnh đầu tiên
        currentImage = ShowRandomImage();

        // Chờ 2.5 giây
        yield return new WaitForSeconds(2.5f);

        // Ẩn ảnh hiện tại
        if (currentImage != null)
            currentImage.SetActive(false);

        // Hiện ảnh random khác
        currentImage = ShowRandomImage();
    }

    void HideAllImages()
    {
        foreach (GameObject img in loadingImages)
        {
            img.SetActive(false);
        }
    }

    GameObject ShowRandomImage()
    {
        if (loadingImages.Length == 0) return null;

        int index = Random.Range(0, loadingImages.Length);
        GameObject img = loadingImages[index];
        img.SetActive(true);
        return img;
    }
}
