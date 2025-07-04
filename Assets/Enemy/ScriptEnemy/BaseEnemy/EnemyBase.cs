using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{

    public Vector3 firstPos;
    public bool hasFirstPos = false;
    public abstract void ResetEnemy();

}
