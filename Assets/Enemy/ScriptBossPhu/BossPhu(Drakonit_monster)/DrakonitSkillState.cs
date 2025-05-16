using System.Collections;
using UnityEngine;

public class DrakonitSkillState : DrakonitState
{
    public float lastSkillTime = 0f; // Thời gian kỹ năng đã sử dụng
   
    public DrakonitSkillState(DrakonitController enemy) : base(enemy) { }
    public override void Enter()
    {
        Debug.Log("trang thai skill");
        enemy.animator.SetBool("Walking", true); // Dừng animation đi bộ
        enemy.agent.isStopped = false; // Cho phép di chuyển
        enemy.isAttack = true;
    }
    public override void Update()
    {
        // Tính khoảng cách giữa enemy và người chơi
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        // Di chuyển liên tục tới vị trí của player
        enemy.agent.SetDestination(enemy.player.position);
        // Nếu người chơi gan, chuyển sang trạng thái attack
        if (distance <= enemy.attackRange && enemy.isAttack)
        {
            enemy.ChangeState(new DrakonitAttackState(enemy));
            return;
        }

        if (Time.time >= lastSkillTime + 20)
        {
            int randomSkill = Random.Range(0, 3); // Chọn ngẫu nhiên kỹ năng từ 0 đến 1
            switch (randomSkill)
            {
                case 0:
                    Debug.Log("skill1");
                    enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi   
                    enemy.auraSkill1.SetActive(true); // Kích hoạt hiệu ứng kỹ năng 1
                    enemy.StartCoroutine(WaitSkill()); // tắt hiệu ứng sau 2 giây                
                    enemy.animator.SetTrigger("Skill1");
                    break;
                case 1:
                    Debug.Log("skill2");
                    enemy.auraSkill2.SetActive(true); // Kích hoạt hiệu ứng kỹ năng 2
                    enemy.StartCoroutine(WaitSkill()); //tắt hiệu ứng sau 2 giây                 
                    enemy.animator.SetTrigger("Skill2");
                    break;
                case 2:
                    Debug.Log("skill3");
                    enemy.StartCoroutine(WaitSkill()); //tắt hiệu ứng sau 2 giây      
                    enemy.animator.SetTrigger("Skill3");
                    break;
            }
            lastSkillTime = Time.time; // Cập nhật thời gian kỹ năng đã sử dụng
        }
    }
       
    public override void Exit()
    {
        enemy.agent.ResetPath(); // Dừng di chuyển nếu thoát khỏi trạng thái
    }
   public IEnumerator WaitSkill()
   {
        enemy.isAttack = false;
        enemy.animator.SetBool("Walking", false); // Dừng animation đi bộ
        enemy.agent.isStopped = true; // ko di chuyển

        yield return new WaitForSeconds(1.5f); // Chờ 1 giây  để tắt aura 
        enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi      
        enemy.auraSkill1.SetActive(false); // Tắt hiệu ứng kỹ năng 1
        enemy.auraSkill2.SetActive(false); // Tắt hiệu ứng kỹ năng 2

        yield return new WaitForSeconds(4f); // Chờ 3 giây để cho di chuyển lại  
        enemy.animator.SetBool("Walking", true); // bat animation đi bộ
        enemy.agent.isStopped = false; // Cho phép di chuyển
        enemy.isAttack = true;
    }

}