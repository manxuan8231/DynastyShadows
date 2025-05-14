using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelAvatar : MonoBehaviour
{
   
    public RawImage rawImage;   
    public Texture2D[] images = new Texture2D[6];  
    [Range(1, 6)]
    public int currentValue = 1;

    //
    public GameObject LVpanel;
    public TextMeshProUGUI[] textLevel;
    public TextMeshProUGUI textScore;
    public int level = 0;
    public int score = 0;
   
    private void Start()
    {
        LVpanel.SetActive(false);
        foreach (TextMeshProUGUI t in textLevel)
        {
            t.text = $"{level}";
        }
        textScore.text = $"Score: {score}";
    }

    void Update()
    {
        int index = Mathf.Clamp(currentValue - 1, 0, images.Length - 1);

        if (images[index] != null)
        {
            rawImage.texture = images[index];
        }
    }



    public void TakeValueAvatar(int amount)
    {
        currentValue -= amount;
        if (currentValue < 1)
        {
            currentValue = images.Length; // quay lại ảnh cuối
            StartCoroutine(WaiForLevelPanel()); // hiện panel khi vòng về cuối
        }

        rawImage.texture = images[currentValue - 1];
    }


    public void AddValueAvatar(int amount)
    {
        currentValue += amount;

        if (currentValue > images.Length)
        {
            currentValue = 1;

            level++; // tăng level
            int randomScore = Random.Range(1, 4);
            score += randomScore;
            foreach (TextMeshProUGUI t in textLevel)
            {
                t.text = $"{level}"; // cập nhật toàn bộ UI
            }
            textScore.text = $"Score: {score}";
            StartCoroutine(WaiForLevelPanel());
        }

        rawImage.texture = images[currentValue - 1];
    }


    public void TakeScore(int amount)
    {
        score -= amount;
        textScore.text = $"Score: {score}";
    }
    private IEnumerator WaiForLevelPanel()
    {
        LVpanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        LVpanel.SetActive(false);
    }
}
