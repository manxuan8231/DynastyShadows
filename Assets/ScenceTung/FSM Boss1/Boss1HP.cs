using UnityEngine;

public class Boss1HP : MonoBehaviour
{
    public int currHp;
    public int maxHp;

    private void Start()
    {
        currHp = maxHp;
    }
}
