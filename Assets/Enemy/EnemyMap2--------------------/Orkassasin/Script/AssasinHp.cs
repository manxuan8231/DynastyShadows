using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AssasinHp : MonoBehaviour,IDamageable
{
    [Header("Hp")]
    public Slider sliderHp;
    public TextMeshProUGUI textHp;
    public int curentHp;
    public int maxHp = 10000;
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
    void Start()
    {
        curentHp = maxHp;
        sliderHp.maxValue = maxHp;
        sliderHp.value = curentHp;
        textHp.text =$"{curentHp}/{maxHp}";
        controllerStateAssa = FindAnyObjectByType<ControllerStateAssa>();
        playerInGame = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
       

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
       
        timeLine.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

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


