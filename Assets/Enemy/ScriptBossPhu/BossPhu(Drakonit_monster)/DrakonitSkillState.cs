using System.Collections;
using UnityEngine;

public class DrakonitSkillState : DrakonitState
{
    public float lastSkillTime = 0f; // Thời gian kỹ năng đã sử dụng
    public DrakonitSkillState(DrakonitController enemy) : base(enemy) { }
    public override void Enter()
    {
       
    }
    public override void Update()
    {
        if (Time.time >= lastSkillTime + 30)
        {
            int randomSkill = Random.Range(0, 2); // Chọn ngẫu nhiên kỹ năng từ 0 đến 1
            switch (randomSkill)
            {
                case 0:
                    Debug.Log("skill1");
                    enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi
                    enemy.auraSkill1.SetActive(true); // Kích hoạt hiệu ứng kỹ năng 1
                    enemy.StartCoroutine(WaitAura()); // tắt hiệu ứng sau 2 giây
                    enemy.animator.SetTrigger("Skill1");
                    break;
                case 1:
                    Debug.Log("skill2");
                    enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi\
                    enemy.auraSkill2.SetActive(true); // Kích hoạt hiệu ứng kỹ năng 2
                    enemy.StartCoroutine(WaitAura()); //tắt hiệu ứng sau 2 giây
                    enemy.animator.SetTrigger("Skill2");
                    break;
                case 2:
                    Debug.Log("skill3");
                    enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi
                    enemy.animator.SetTrigger("Skill3");
                    break;
            }
            lastSkillTime = Time.time; // Cập nhật thời gian kỹ năng đã sử dụng
        }
        else
        {
          
        }
    }
    public override void Exit()
    {
        enemy.ChangeState(new DrakonitChaseState(enemy)); // Chuyển sang trạng thái Chase
    }
   public IEnumerator WaitAura()
    {
        yield return new WaitForSeconds(2f); // Chờ 1 giây
        enemy.auraSkill1.SetActive(false); // Tắt hiệu ứng kỹ năng 1
        enemy.auraSkill2.SetActive(false); // Tắt hiệu ứng kỹ năng 2
    }
}