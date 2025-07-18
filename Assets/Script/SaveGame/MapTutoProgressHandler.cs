public static class MapTutoProgressHandler
{
    /// Ghi chỉ số bước map‑tutorial
    public static void SaveIndex(int idx)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        data.mapTutoIndex = idx;
        SaveManagerMan.SaveGame(data);
       
    }

    /// Đọc chỉ số đã lưu (trả về 0 nếu chưa có)
    public static int LoadIndex()
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        return data.mapTutoIndex;
    }
}
