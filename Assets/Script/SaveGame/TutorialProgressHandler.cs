using UnityEngine;

public static class TutorialProgressHandler
{
    private const string TutorialKey = "HasSeenTutorial";

    public static bool IsTutorialDone()
    {
        return PlayerPrefs.GetInt(TutorialKey, 0) == 1;
    }

    public static void MarkTutorialDone()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();
    }

    public static void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(TutorialKey);
    }
}
