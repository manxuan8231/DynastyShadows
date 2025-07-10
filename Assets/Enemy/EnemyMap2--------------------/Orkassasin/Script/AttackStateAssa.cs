using Pathfinding;
using System.Collections;
using UnityEngine;

public class AttackStateAssa : AssasinState
{
    public AttackStateAssa(ControllerStateAssa enemy): base(enemy){ }
   
    public override void Enter()
    {
        enemy.aiPath.maxSpeed = 7f;
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isMoveLeft", false);
        enemy.animator.SetBool("isMoveRight", false);
        enemy.animator.SetBool("isMoveBack", false);
        enemy.aiPath.enableRotation = true;
    }

    public override void Update()
    {
        float distan = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distan <= 4 && Time.time >= enemy.lastAttackTime + enemy.coolDownAttack)
        {
            FlipToPlayer();
            enemy.aiPath.enableRotation = true;
           
            if (enemy.stepAttack == 0)
            {
                enemy.stepAttack++;
                
                enemy.animator.SetTrigger("Attack1");
                enemy.StartCoroutine(WaitCanMove(1.5f));
            }
            else if (enemy.stepAttack == 1)
            {
                enemy.stepAttack++;
                enemy.animator.SetTrigger("Attack2");
                enemy.StartCoroutine(WaitCanMove(1.5f));
            }
            else if (enemy.stepAttack == 2)
            {
                enemy.stepAttack = 0;
                enemy.animator.SetTrigger("Attack3");
                enemy.StartCoroutine(WaitCanMove(1.5f));
            }
            enemy.lastAttackTime = Time.time;
        }
        else if(distan > 3 && distan < 10)
        {
            MoveBackRightLeft();
            
        }

        else if (distan >= 10)//neu di qua 8f thi chuyen trang thai
        {
            enemy.ChangeState(new CurrentStateAssa(enemy));
        }
    }


    public void MoveBackRightLeft()
    {
        FlipToPlayer(); // Xoay mặt về player
        enemy.aiPath.enableRotation = false;

        // Tính hướng forward (hướng từ enemy → player)
        Vector3 forward = (enemy.player.transform.position - enemy.transform.position).normalized;
        forward.y = 0f;

        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 chosenDir = Vector3.zero;

        float moveDistance = 5f;//met

        switch (enemy.stepAttack)
        {
            case 0: 
                chosenDir = -forward;
                enemy.animator.SetBool("isMoveBack", true);
                enemy.animator.SetBool("isMoveLeft", false);
                enemy.animator.SetBool("isMoveRight", false);
                break;

            case 1:
                chosenDir = -right;
                enemy.animator.SetBool("isMoveBack", false);
                enemy.animator.SetBool("isMoveLeft", true);
                enemy.animator.SetBool("isMoveRight", false);
                break;

            case 2: 
                chosenDir = right;
                enemy.animator.SetBool("isMoveBack", false);
                enemy.animator.SetBool("isMoveLeft", false);
                enemy.animator.SetBool("isMoveRight", true);
                break;
        }

        Vector3 targetPos = enemy.transform.position + chosenDir * moveDistance;
        enemy.aiPath.destination = targetPos;
    }

    public IEnumerator WaitCanMove(float second)
   {
        enemy.aiPath.canMove = false;
        yield return new WaitForSeconds(second);
        enemy.aiPath.canMove = true;
   }


    public void FlipToPlayer()
    {
        Vector3 direction =enemy. player.transform.position - enemy.transform.position;

        // Không xoay theo trục dọc
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = lookRotation;
    }

}
