using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class SkillData
{
    public string skillID;
    public float cooldownDuration = 5f;
    [HideInInspector] public float cooldownTimer = 0f;
    public Slider cooldownSlider;
}
