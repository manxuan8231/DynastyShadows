using UnityEngine;

public class SlowMotionDodgeEvent : MonoBehaviour
{
    public bool isDodgeWindowActive = false;
    private float dodgeTimer = 0f;
    public float maxDodgeTime = 2f;
    public bool isOneSlow = true;
    private void Update()
    {
        if (isDodgeWindowActive)
        {
            isOneSlow = false;
            Time.timeScale = 0.1f;
            dodgeTimer += Time.unscaledDeltaTime;

            // Nếu quá thời gian mà chưa né thì thoát
            if (dodgeTimer >= maxDodgeTime)
            {
                ResetTime();
            }
        }
    }

    public void ResetTime()
    {
        isDodgeWindowActive = false;
        Time.timeScale = 1f;
        dodgeTimer = 0f;
    }
}
