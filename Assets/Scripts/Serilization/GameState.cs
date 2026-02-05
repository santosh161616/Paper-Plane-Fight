using UnityEngine;
using System;
using Plane.Utils;

public class GameState : SingletonMonoBehaviour<GameState> 
{

    [SerializeField] private GameConfig config;

    public GameConfig Config => config;

    public bool IsDebug => config.IsDebug;

    public event Action OnGameInitialized;

    private void Initialize()
    {
        ApplyEnvironmentRules();
        OnGameInitialized?.Invoke();
    }

    private void ApplyEnvironmentRules()
    {
        if (!config.enableLogs)
        {
            Debug.unityLogger.logEnabled = false;
        }

        if (IsDebug)
        {
            Time.timeScale = 1f;
            Debug.Log("🚀 Debug Mode Enabled");
        }
    }
}
