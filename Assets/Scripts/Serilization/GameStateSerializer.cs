using UnityEngine;

[System.Serializable]
public class SerializedGameState
{
    public int version;
    public string buildEnv;
    public bool cheatsUsed;
}

public static class GameStateSerializer
{
    private const string SaveKey = "GAME_STATE";

    public static void Save(SerializedGameState state)
    {
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static SerializedGameState Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return CreateDefault();

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<SerializedGameState>(json);
    }

    private static SerializedGameState CreateDefault()
    {
        return new SerializedGameState
        {
            version = 1,
            buildEnv = GameState.Instance.Config.environment.ToString(),
            cheatsUsed = false
        };
    }
}
