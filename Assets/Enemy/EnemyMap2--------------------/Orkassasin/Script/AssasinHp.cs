using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;

public class AssasinHp : MonoBehaviour,IDamageable
{
    [Header("Hp")]
    public Slider sliderHp;
    public Slider eaSliderHp;
    public TextMeshProUGUI textHp;
    public int curentHp;
    public int maxHp = 10000;
    public float lerpSpeed = 0.05f; // Tốc độ lerp cho thanh máu
    [Header("tranh ne ")]
    public float scoreDodge = 0f;
    //tham chieu
    public ControllerStateAssa controllerStateAssa;

    [Header("TimeLine")]
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public PlayableDirector playableDirector;
    bool isTimeLine = false;
    public bool isQuestDone = false;
    [Header("Video")]
    public GameObject mainCameraEnd;

    void Start()
    {
        curentHp = maxHp;
        sliderHp.maxValue = curentHp;
        sliderHp.value = curentHp;
        eaSliderHp.maxValue = curentHp;
        eaSliderHp.value = curentHp;
        textHp.text =$"{curentHp}/{maxHp}";
        controllerStateAssa = FindAnyObjectByType<ControllerStateAssa>();
        playerInGame = GameObject.FindGameObjectWithTag("Player");
        mainCameraEnd.SetActive ( false); // Tắt camera chính khi bắt đầu timeline
    }

    
    void Update()
    {
        if (sliderHp.value != eaSliderHp.value) { 
            eaSliderHp.value = Mathf.Lerp(eaSliderHp.value, curentHp, lerpSpeed);
        }

    }
    public void TakeDamage(float damage)
    {
        curentHp -= (int)damage;
        curentHp = Mathf.Clamp(curentHp, 0, maxHp);
        UpdateUI();

        if (curentHp <= 0 && !isTimeLine)
        {
            StartCoroutine(StartTimeLineEnd()); 
            Destroy(gameObject,2f);
        }
    }
    private void OnTimelineFinished(PlayableDirector director)
    {
     
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine.SetActive(false);
        isQuestDone = true; // Đánh dấu nhiệm vụ đã hoàn thành
        timeLine.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc
        mainCameraEnd.SetActive(true);
        GameSaveData data = SaveManagerMan.LoadGame();
        data.dataQuest.isQuestMap2 = isQuestDone; // Cập nhật trạng thái nhiệm vụ
        DataQuestSingleTon.isQuestMap2 = isQuestDone; // Cập nhật trạng thái nhiệm vụ trong singleton
        SaveManagerMan.SaveGame(data); // Lưu dữ liệu nhiệm vụ
    }
    void UpdateUI()
    {
        sliderHp.value = curentHp;
        textHp.text = $"{curentHp}/{maxHp}";
    }

    IEnumerator StartTimeLineEnd()
    {
        isTimeLine = true; // Đánh dấu là đã kích hoạt timeline
        yield return new WaitForSeconds(0.3f);
        playerInGame.SetActive(false); // Ẩn player thật
        timeLine.SetActive(true); // Bật timeline
        playableDirector.Play(); // Chạy timeline
        playableDirector.stopped += OnTimelineFinished; // Đăng ký sự kiện khi timeline kết thúc
    }
}


