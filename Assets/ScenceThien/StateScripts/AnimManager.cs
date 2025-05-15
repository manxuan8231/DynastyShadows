using UnityEngine;

public static class AnimatorExtensions
{
    public static void ResetAllTriggers(this Animator anim)
    {
            foreach (var param in anim.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    anim.ResetTrigger(param.name);
                }
            }
    }
}