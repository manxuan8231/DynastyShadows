using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyPrefabs : MonoBehaviour
{
    public GameObject playerPre;
    private PlayerControllerState player;
    public bool isLoadScene = false;
    private void Start()
    {
        player = FindAnyObjectByType<PlayerControllerState>();
      
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && isLoadScene)
        {
            //skill4
            player.animator.runtimeAnimatorController = player.animatorDefauld; // Trở về animator mặc định
            player.animator.SetTrigger("Skill4State");
            player.skill4Manager.isHibitedIcon = false; // cam skill icon
            player.isSkinSkill3Clone = false;
            //skill2
            player.skill2Manager.isHibitedIcon = false; // Bỏ cấm sử dụng skill 2
            player.ChangeState(new PlayerCurrentState(player)); // Trở về trạng thái hiện tại

            DontDestroyOnLoad(playerPre);
            SceneManager.sceneLoaded += OnScenceLoad;
            SceneManager.LoadScene("Map2");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //skill4
            player.animator.runtimeAnimatorController = player.animatorDefauld; // Trở về animator mặc định
            player.animator.SetTrigger("Skill4");
            player.skill4Manager.isHibitedIcon = false; // cam skill icon
            player.isSkinSkill3Clone = false;
            //skill2
            player.skill2Manager.isHibitedIcon = false; // Bỏ cấm sử dụng skill 2
            player.ChangeState(new PlayerCurrentState(player)); // Trở về trạng thái hiện tại

            DontDestroyOnLoad(playerPre);
            SceneManager.sceneLoaded += OnScenceLoad;
            SceneManager.LoadScene("Map2");
        }
    }
    public void OnScenceLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnScenceLoad;
       
    }

   
}