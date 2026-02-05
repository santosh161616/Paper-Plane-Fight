using UnityEngine;

[CreateAssetMenu(
    fileName = "GameConfig", menuName = "Core/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Build")]
    public GameMode gameMode;
    public BuildEnvironment environment;

    [Header("Debug Features")]
    public bool enableCheats;
    public bool showFPS;
    public bool enableLogs;

    [Header("Monetization")]
    public bool adsEnabled;
    public bool iapEnabled;

    [Header("Analytics")]
    public bool analyticsEnabled;

    public bool IsDebug => gameMode == GameMode.Debug;
}


// GameMode and BuildEnvironment enums
public enum GameMode
{
    Debug,
    Release
}

public enum BuildEnvironment
{
    Development,
    Staging,
    Production
}
