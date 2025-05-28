using UnityEngine;

[CreateAssetMenu(fileName = "BossAnimationData", menuName = "BossAnimationData/BossAnimationData")]
public class BossAnimationData : ScriptableObject
{
    public string idle;
    public string chase;
    public string[] attackPhase1;
    public string[] attackPhase2;
    public string death;
    public string phaseChangeState;
    public string gethit;
    public string idlecombat;
}
