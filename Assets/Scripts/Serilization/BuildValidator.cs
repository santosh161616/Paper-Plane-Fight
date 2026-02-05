#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class BuildValidator
{
    static BuildValidator()
    {
        EditorApplication.delayCall += Validate;
    }

    static void Validate()
    {
        var config = Resources.Load<GameConfig>("GameConfig");
        if (config != null && config.gameMode == GameMode.Debug)
        {
            Debug.LogWarning("⚠️ GameConfig is in DEBUG mode!");
        }
    }
}
#endif
