using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Plane.Utils
{
    public class SceneNavigator : EditorWindow
    {
        private Vector2 scrollPosition;
        private string searchFilter = "";
        private List<string> favoriteScenes = new List<string>();
        private GUIStyle headerStyle;
        private GUIStyle sceneButtonStyle;
        private Color favoriteColor = new Color(1f, 0.85f, 0.4f);

        [MenuItem("Tools/Scene Navigator ✨", priority = 1)]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneNavigator>("Scene Navigator");
            window.minSize = new Vector2(350, 500);
        }

        private void OnGUI()
        {
            float buttonHeight = 22f;
            float smallButtonWidth = 25f;

            GUIStyle favButtonStyleFixed = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                padding = new RectOffset(6, 6, 2, 2) // left & right padding
            };
            // Initialize styles here (safe)
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (sceneButtonStyle == null)
            {
                sceneButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 12
                };
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("🎮 Scene Navigator", headerStyle);
            EditorGUILayout.Space();

            // Search bar
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField("🔍 Search:", GUILayout.Width(60));
            searchFilter = EditorGUILayout.TextField(searchFilter);
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
                searchFilter = "";
            EditorGUILayout.EndHorizontal();

            // Get all active build scenes
            string[] activeScenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"📂 Total Scenes: {activeScenes.Length}", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Favorites
            if (favoriteScenes.Count > 0)
            {
                EditorGUILayout.LabelField("⭐ Favorites", EditorStyles.boldLabel);
                //bool isOpen = activeScenes.Contains(scenePath);

                foreach (string fav in favoriteScenes.ToArray())
                {
                    EditorGUILayout.BeginHorizontal();
                    //GUI.backgroundColor = favoriteColor;
                    //if (GUILayout.Button(System.IO.Path.GetFileNameWithoutExtension(fav), sceneButtonStyle))
                    //    OpenScene(fav);
                    //GUI.backgroundColor = Color.white;

                    //--------------//
                    GUILayout.Space(10); // left margin

                    // Scene name button
                    //GUI.backgroundColor = isOpen ? new Color(0.45f, 0.85f, 0.45f) : favoriteColor;
                    if (GUILayout.Button($"▶  {System.IO.Path.GetFileNameWithoutExtension(fav)}", favButtonStyleFixed, GUILayout.Height(buttonHeight)))
                        OpenScene(fav);
                    //GUI.backgroundColor = Color.lightBlue;

                    GUILayout.Space(4);
                    //--------------//

                    if (GUILayout.Button("❌", GUILayout.Width(25)))
                        favoriteScenes.Remove(fav);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            // Scene List
            EditorGUILayout.LabelField("📜 Active Build Scenes", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Color defaultBg = GUI.backgroundColor;
            Color openSceneColor = new Color(0.5f, 0.9f, 0.5f); // light green for open scenes
            Color lineColor = new Color(0.3f, 0.3f, 0.3f, 0.4f); // separator line
            Color lightGreen = new Color(0.45f, 0.85f, 0.45f);

            // Get open scenes
            var openSceneNames = Enumerable.Range(0, EditorSceneManager.sceneCount)
                .Select(i => EditorSceneManager.GetSceneAt(i).path)
                .ToHashSet();

            // Styles
            GUIStyle sceneButtonStyleFixed = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 12,
                padding = new RectOffset(6, 6, 2, 2) // left & right padding
            };
            GUIStyle smallButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            //float buttonHeight = 22f;
            //float smallButtonWidth = 25f;

            foreach (string scenePath in activeScenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (!string.IsNullOrEmpty(searchFilter) && !sceneName.ToLower().Contains(searchFilter.ToLower()))
                    continue;

                bool isOpen = openSceneNames.Contains(scenePath);

                Rect rowRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(buttonHeight));

                // Background highlight for open scene
                //if (Event.current.type == EventType.Repaint && isOpen)
                //    EditorGUI.DrawRect(rowRect, lightGreen);

                GUILayout.Space(10); // left margin

                // Scene name button
                GUI.backgroundColor = isOpen ? new Color(0.45f, 0.85f, 0.45f) : defaultBg;
                if (GUILayout.Button($"▶  {sceneName}", sceneButtonStyleFixed, GUILayout.Height(buttonHeight)))
                    OpenScene(scenePath);
                GUI.backgroundColor = defaultBg;

                GUILayout.Space(4);
                if (GUILayout.Button("▶", smallButtonStyle, GUILayout.Width(smallButtonWidth), GUILayout.Height(buttonHeight)))
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorApplication.isPlaying = true;


                GUILayout.Space(4); // spacing between buttons

                // Additive button
                if (GUILayout.Button("➕", smallButtonStyle, GUILayout.Width(smallButtonWidth), GUILayout.Height(buttonHeight)))
                    OpenSceneAdditive(scenePath);

                GUILayout.Space(4);

                // Favorite button
                if (GUILayout.Button("⭐", smallButtonStyle, GUILayout.Width(smallButtonWidth), GUILayout.Height(buttonHeight)))
                {
                    if (!favoriteScenes.Contains(scenePath))
                        favoriteScenes.Add(scenePath);
                }

                EditorGUILayout.EndHorizontal();

                // Separator line
                Rect lineRect = EditorGUILayout.GetControlRect(false, 1);
                EditorGUI.DrawRect(lineRect, lineColor);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);



            // Play from first scene
            if (activeScenes.Length > 0)
            {
                string firstScene = activeScenes[0];
                if (GUILayout.Button("🚀 Play From First Scene", GUILayout.Height(30)))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(firstScene);
                        EditorApplication.isPlaying = true;
                    }
                }
            }

            // Utility buttons
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("🔄 Refresh"))
                Repaint();
            if (GUILayout.Button("🧹 Clear Favorites"))
                favoriteScenes.Clear();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Tip: ⭐ Pin frequently used scenes for quick access!", MessageType.Info);
        }

        private void OpenScene(string scenePath)
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog(
                    "Cannot Open Scene",
                    "Opening scenes is not allowed while in Play Mode!",
                    "OK"
                );
                return;
            }
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
                Debug.Log($"Opened Scene: {scenePath}");
            }
        }

        private void OpenSceneAdditive(string scenePath)
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog(
                    "Cannot Open Scene",
                    "Opening scenes additively is not allowed while in Play Mode!",
                    "OK"
                );
                return;
            }
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                Debug.Log($"Opened Additively: {scenePath}");
            }
        }

    }
}
