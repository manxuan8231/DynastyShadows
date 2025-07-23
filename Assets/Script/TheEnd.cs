using System.Collections;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
    public GameObject btnQuit; // Nút thoát game
    public GameObject btnTurnOff; // Nút ẩn đối tượng sau khi thực hiện hành động
    public float time = 15f; // Thời gian đợi trước khi thực hiện hành động
    public GameObject playerInGame;
    void Start()
    {
        playerInGame = GameObject.FindGameObjectWithTag("Player");
        btnQuit.SetActive(false); // Ẩn nút thoát game ban đầu
        btnTurnOff.SetActive(false); // Ẩn nút ẩn đối tượng ban đầu
        StartCoroutine(WaitTheEnd( time));
    }

   
   
    public IEnumerator WaitTheEnd(float time)
    {
        playerInGame.SetActive(false); // Ẩn đối tượng player trong game

        yield return new WaitForSeconds(time);

       btnQuit.SetActive(true); // Hiển thị nút thoát game sau thời gian chờ
        btnTurnOff.SetActive(true); // Hiển thị nút ẩn đối tượng sau thời gian chờ
    } 
    public void ButtonTurnOff()
    {
        playerInGame.SetActive(true);
        gameObject.SetActive(false); // Ẩn đối tượng sau khi thực hiện hành động

    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // Thoát khỏi ứng dụng

    }
}
