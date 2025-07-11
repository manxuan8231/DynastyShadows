using Pathfinding;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class SkillKnifeStateAssa : AssasinState
{
    public SkillKnifeStateAssa(ControllerStateAssa enemy) : base(enemy) { }
   

    public override void Enter()
    {
       
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isMoveLeft", false);
        enemy.animator.SetBool("isMoveRight", false);
    }

    public override void Update()
    {
        FlipToPlayer();
        float dis = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (dis <= 10 && Time.time >= enemy.lastTimeSkillDash + enemy.cooldownSkillDash)
        {

            enemy.StartCoroutine(WaitDash());
            enemy.randomMoveSkillDash = Random.Range(0, 2);
            enemy.lastTimeSkillDash = Time.time;
        }
        else if(dis <= 10 && Time.time <= enemy.lastTimeSkillDash + enemy.cooldownSkillDash)//khoan cach thay player la 10 va dg trong thoi gian cooldown ms cho move
        {
            
            Vector3 forward = (enemy.player.transform.position - enemy.transform.position).normalized;
            forward.y = 0f;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 chosenDir = Vector3.zero;
            enemy.aiPath.enableRotation = false;
            if (enemy.randomMoveSkillDash == 0) 
            {
                chosenDir = -right;
                enemy.animator.SetBool("isMoveLeft", true);
                enemy.animator.SetBool("isMoveRight", false);
                enemy.animator.SetBool("isRunForward", false);
            }
            else if(enemy.randomMoveSkillDash == 1)
            {
                chosenDir = right;
                enemy.animator.SetBool("isMoveRight", true);
                enemy.animator.SetBool("isRunForward", false);
                enemy.animator.SetBool("isMoveLeft", false);
               
            }
            // Di chuyển theo hướng đã chọn
            Vector3 targetPos = enemy.transform.position + chosenDir * 10f;
            enemy.aiPath.destination = targetPos;
        }
        //nếu player xa quá 10m thi chay toi
        if (dis > 10) 
        {
            enemy.animator.SetBool("isRunForward", true);
            enemy.animator.SetBool("isMoveRight", false);
            enemy.animator.SetBool("isMoveLeft", false);
            enemy.aiPath.maxSpeed = 10f;
            enemy.aiPath.destination = enemy.player.transform.position;
        }
        else
        {
            enemy.aiPath.maxSpeed = 5f;
            enemy.animator.SetBool("isRunForward", false);
        }
    }
    public void FlipToPlayer()
    {
        Vector3 direction = enemy.player.transform.position - enemy.transform.position;

        // Không xoay theo trục dọc
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = lookRotation;
    }

    public IEnumerator WaitDash()
    {
        enemy.animator.SetTrigger("WaitDash");
        yield return new WaitForSeconds(0.6f);
        Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
        Vector3 final = enemy.transform.position + direction * 20f;
        enemy.StartCoroutine(DashCaculator(final, 0.2f));
        enemy.animator.SetTrigger("Dash");
    }
    public IEnumerator DashCaculator(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = enemy.transform.position;
        float time = 0f;
        enemy.aiPath.enabled = false;
        enemy.boxTakeDame.enabled = false;
        while (time < duration) 
        { 
            time += Time.deltaTime;
            float t = time / duration;
            enemy.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }
        enemy.transform.position = targetPosition;
        enemy.aiPath.enabled = true;
        enemy.boxTakeDame.enabled = true;
    }
    public IEnumerator WaitCanMove(float second)
    {
        enemy.aiPath.canMove = false;
        yield return new WaitForSeconds(second);
        enemy.aiPath.canMove = true;
    }

}
