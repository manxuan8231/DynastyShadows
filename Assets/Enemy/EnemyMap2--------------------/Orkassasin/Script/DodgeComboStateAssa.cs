using System.Collections;
using UnityEngine;

public class DodgeComboStateAssa : AssasinState
{
    public DodgeComboStateAssa(ControllerStateAssa enemy) : base(enemy) { }
   
    public override void Enter()
    {
        enemy.aiPath.canMove = false;
        enemy.StartCoroutine(WaitChangeState());
    }

    public override void Exit()
    {
        enemy.aiPath.canMove = true;
    }

    public override void Update()
    {
        FlipToPlayer();
    }
    public IEnumerator WaitChangeState()
    {
        // Dash lùi khỏi player
        Vector3 dirAwayFromPlayer = (enemy.transform.position - enemy.player.transform.position).normalized;
        Vector3 dashBackPos = enemy.transform.position + dirAwayFromPlayer * 20f;
        dashBackPos.y = enemy.transform.position.y; // giữ nguyên độ cao
        yield return enemy.StartCoroutine(DashCaculator(dashBackPos, 0.2f));

        // Phóng dao
        enemy.animator.SetTrigger("knifeThrower");
       
        yield return new WaitForSeconds(1f);

        //Dịch chuyển tới trước mặt player 
        Vector3 dirToPlayer = (enemy.player.transform.position - enemy.transform.position).normalized;
        Vector3 attackFronPl = enemy.player.transform.position - dirToPlayer * 4f;//tinh toan trc mat pl 4m
        attackFronPl.y = enemy.transform.position.y; // giữ nguyên độ cao
        enemy.transform.position = attackFronPl;

        enemy.StartCoroutine(enemy.PrepareThenAttack());// Chuẩn bị tấn công
        enemy.animator.SetTrigger("attack11");
        yield return new WaitForSeconds(1f);

        // dash sang phải 
        
        Vector3 right = enemy.transform.right;
        Vector3 dashRightPos = enemy.transform.position + right * 20f;
        dashRightPos.y = enemy.transform.position.y; // giữ nguyên độ cao
        yield return enemy.StartCoroutine(DashCaculator(dashRightPos, 0.2f));

        // Phóng dao 
        enemy.animator.SetTrigger("knifeThrower");       
        yield return new WaitForSeconds(1f);

        //  tan cong ben phai pl      
        Vector3 rightOfPlayer = enemy.player.transform.right * 4f;
        Vector3 targetPosition = enemy.player.transform.position - rightOfPlayer;
        targetPosition.y = enemy.transform.position.y; // giữ nguyên độ cao
        enemy.transform.position = targetPosition;
        enemy.StartCoroutine(enemy.PrepareThenAttack());// Chuẩn bị tấn công
        enemy.animator.SetTrigger("attack16");
        yield return new WaitForSeconds(1f);

        //dash qua trai      
        Vector3 left = -enemy.transform.right ;
        Vector3 final = enemy.transform.position + left * 20f;
        yield return enemy.StartCoroutine(DashCaculator(final, 0.2f));

        // Phóng dao 
        enemy.animator.SetTrigger("knifeThrower");    
        yield return new WaitForSeconds(1f);

        // chuyển trạng thái
        enemy.ChangeState(new CurrentStateAssa(enemy));
    }

    public IEnumerator DashCaculator(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = enemy.transform.position;
        float time = 0f;

        enemy.aiPath.enabled = false;
        enemy.boxTakeDame.enabled = false;
        enemy.evenAnimatorAssa.PlayDashSound();

        Vector3 dashDir = (targetPosition - startPos).normalized;
        float dashDistance = Vector3.Distance(startPos, targetPosition);

        LayerMask mask = LayerMask.GetMask("Ground", "Obstacle", "Wall");

        // Thời gian tạo ảnh mỗi 0.4 giây
        float afterImageTimer = 0f;
        float afterImageInterval = 0.05f;

        while (time < duration)
        {
            time += Time.deltaTime;
            afterImageTimer += Time.deltaTime;

            float t = time / duration;
            Vector3 nextPos = Vector3.Lerp(startPos, targetPosition, t);
            nextPos.y = startPos.y;

            Vector3 rayOrigin = enemy.transform.position + Vector3.up * 2f;
            Debug.DrawRay(rayOrigin, dashDir * 5f, Color.green, 0.1f);

            if (Physics.Raycast(rayOrigin, dashDir, out RaycastHit hit, 1f, mask))
            {
                break;
            }

            // Chỉ tạo ảnh nếu đã đủ thời gian
            if (afterImageTimer >= afterImageInterval)
            {
                enemy.evenAnimatorAssa.CreateAsterImg();
                afterImageTimer = 0f;
            }

            enemy.transform.position = nextPos;
            yield return null;
        }

        // Bật lại AI sau dash hoặc khi bị cản
        enemy.aiPath.enabled = true;
        enemy.boxTakeDame.enabled = true;
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
}
