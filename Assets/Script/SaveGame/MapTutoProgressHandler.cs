using UnityEngine;

public static class MapTutoProgressHandler
{
    private const string MapTutoKey = "MapTutoIndex";

    public static void SaveIndex(int index)
    {
        PlayerPrefs.SetInt(MapTutoKey, index);
        PlayerPrefs.Save();
    }

    public static int LoadIndex()
    {
        return PlayerPrefs.GetInt(MapTutoKey, 0); // Mặc định là 0 nếu chưa từng lưu
    }

    public static void ResetIndex()
    {
        PlayerPrefs.DeleteKey(MapTutoKey);
    }
}
