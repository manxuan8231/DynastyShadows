
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillKnifeStateAssa : AssasinState
{
    public SkillKnifeStateAssa(ControllerStateAssa enemy) : base(enemy) { }
   

    public override void Enter()
    {
       enemy.StartCoroutine(WaitWalkBack());
    }

    public override void Exit()
    {
        enemy.animator.SetBool("isMoveLeft", false);
        enemy.animator.SetBool("isMoveRight", false);
        enemy.animator.SetBool("isWalkForward", false);
        enemy.animator.SetBool("isMoveBack", false);
        int layerDefaul = LayerMask.NameToLayer("Enemy");
        SetLayerRecursively(enemy.gameObject, layerDefaul);
    }

    public override void Update()
    {   
       
    
        FlipToPlayer();
        
        if (enemy.isMoveBack) return;
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
                enemy.animator.SetBool("isMoveBack", false);
                enemy.animator.SetBool("isWalkForward", false);
            }
            else if(enemy.randomMoveSkillDash == 1)
            {
                chosenDir = right;
                enemy.animator.SetBool("isMoveRight", true);
                enemy.animator.SetBool("isWalkForward", false);
                enemy.animator.SetBool("isMoveBack", false);
                enemy.animator.SetBool("isMoveLeft", false);
               
            }
            // Di chuyển theo hướng đã chọn
            Vector3 targetPos = enemy.transform.position + chosenDir * 10f;
            enemy.aiPath.destination = targetPos;
        }
        //nếu player xa quá 10m thi chay toi
        if (dis > 10) 
        {
            enemy.animator.SetBool("isWalkForward", true);
            enemy.animator.SetBool("isMoveRight", false);
            enemy.animator.SetBool("isMoveLeft", false);
            enemy.animator.SetBool("isMoveBack", false);
            enemy.aiPath.maxSpeed = 20f;
            enemy.aiPath.destination = enemy.player.transform.position;
        }
        else
        {
            enemy.aiPath.maxSpeed = 7f;
            enemy.animator.SetBool("isWalkForward", false);
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

   

    //nếu muốn đổi layer của gameobject và tất cả con của nó thì kêu hàm này kem theo
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    //IEnumera
    public IEnumerator WaitDash()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("ShadowAssa");
        if (shadows.Length >= 5)
        {
            yield return enemy.StartCoroutine(MultiShadowDash());
        }
        else
        {       
            enemy.animator.SetTrigger("WaitDash");
            yield return new WaitForSeconds(0.3f);
            enemy.evenAnimatorAssa.StartShadow(); //  tạo ảo ảnh nếu chưa đủ
            Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            Vector3 final = enemy.transform.position + direction * 20f;
            enemy.StartCoroutine(DashCaculator(final, 0.2f));
            enemy.animator.SetTrigger("Dash");
        }

    }
    public IEnumerator DashCaculator(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = enemy.transform.position;
        float time = 0f;

        enemy.aiPath.enabled = false;
        enemy.boxTakeDame.enabled = false;

        Vector3 dashDir = (targetPosition - startPos).normalized;
        float dashDistance = Vector3.Distance(startPos, targetPosition);

        LayerMask mask = LayerMask.GetMask("Ground", "Obstacle");

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            Vector3 nextPos = Vector3.Lerp(startPos, targetPosition, t);
            nextPos.y = startPos.y;

            Vector3 rayOrigin = enemy.transform.position + Vector3.up * 2f;
            Debug.DrawRay(rayOrigin, dashDir * 5f, Color.green, 0.1f);

            if (Physics.Raycast(rayOrigin, dashDir, out RaycastHit hit, 1f, mask))
            {
                break;
            }

           enemy.StartCoroutine( WaitOffAuraDash());
            enemy.transform.position = nextPos;
            yield return null;
        }

        // Bật lại AI sau dash hoặc khi bị cản
        enemy.aiPath.enabled = true;
        enemy.boxTakeDame.enabled = true;
    }
    public IEnumerator WaitCanMove(float second)
    {
        enemy.aiPath.canMove = false;
        yield return new WaitForSeconds(second);
        enemy.aiPath.canMove = true;
    }
    public IEnumerator MultiShadowDash()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("ShadowAssa");

        if (shadows.Length < 5)
            yield break; // Không đủ shadow

        // Lấy 5 shadow gần nhất
        List<GameObject> sortedShadows = new List<GameObject>(shadows);
        sortedShadows.Sort((a, b) =>
            Vector3.Distance(enemy.transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(enemy.transform.position, b.transform.position)));

        for (int i = 0; i < 5; i++)
        {
            GameObject shadow = sortedShadows[i];

            // Teleport enemy đến shadow
            enemy.aiPath.enabled = false;
            enemy.transform.position = shadow.transform.position;
            ShadowAssa shadowAssa = shadow.GetComponent<ShadowAssa>();
            if (shadowAssa != null) { shadowAssa.Disappear(); }//destroy
            yield return new WaitForSeconds(0.1f);

            // Dash từ shadow tới player
            Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            Vector3 final = enemy.transform.position + direction * 40f;
            enemy.animator.SetTrigger("Dash");
            yield return DashCaculator(final, 0.2f);

            yield return new WaitForSeconds(0.2f); // delay giữa mỗi lần dash
        }

        // Tele đến vị trí trước mặt player cách 3f
        Vector3 faceDir = (enemy.player.transform.position - enemy.transform.position).normalized;
        faceDir.y = 0f;
        Vector3 finalPos = enemy.player.transform.position - faceDir * 3f;

        enemy.transform.position = finalPos;
        enemy.transform.rotation = Quaternion.LookRotation(enemy.player.transform.position - enemy.transform.position);

        // Gọi đòn tấn công cuối
        enemy.animator.SetTrigger("FinalAttackDash");

        // Chuyển sang trạng thái khác sau animation
        yield return new WaitForSeconds(1f); 
        enemy.ChangeState(new CurrentStateAssa(enemy));
    }
    public IEnumerator WaitWalkBack()//dợi đi lùi r ms cho sử dụng skill 
    {
        enemy.isMoveBack = true;
        enemy.aiPath.enableRotation = false;
        Vector3 back = (enemy.transform.position - enemy.player.transform.position).normalized;
        Vector3 final = enemy.transform.position + back * 10f;
        enemy.animator.SetBool("isMoveBack", true);
        enemy.aiPath.destination = final;
       
        yield return new WaitForSeconds(2f);
        enemy.aiPath.enableRotation = true;
        int layerNew = LayerMask.NameToLayer("InvisibleAssasin");
        SetLayerRecursively(enemy.gameObject, layerNew);
        yield return new WaitForSeconds(1f);
        enemy.isMoveBack = false; 
    }
    public IEnumerator WaitOffAuraDash()
    {
        int layerDefau = LayerMask.NameToLayer("Enemy");
        SetLayerRecursively(enemy.gameObject, layerDefau);
        enemy.auraDash.SetActive(true);
        yield return new WaitForSeconds(1f);
        enemy.auraDash.SetActive(false);
        int layerNew = LayerMask.NameToLayer("InvisibleAssasin");
        SetLayerRecursively(enemy.gameObject, layerNew);
    }

}
