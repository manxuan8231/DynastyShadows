public static class TutorialProgressHandler
{
    public static void MarkTutorialDone()
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        data.isTutorialDone = true;
        
        SaveManagerMan.SaveGame(data);
        
    }

    public static bool IsTutorialDone()
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        return data.isTutorialDone;
    }
}
