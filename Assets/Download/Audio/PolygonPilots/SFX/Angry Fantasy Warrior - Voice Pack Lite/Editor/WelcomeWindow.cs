using System.IO;
using UnityEditor;
using UnityEngine;

namespace ResonantMesh.AngryBarbarianVoicePack
{
    [InitializeOnLoad]
    public class WelcomeWindow : EditorWindow
    {
        // Unique key to prevent the window from reopening on every domain reload
        private const string PrefKeyShown = "PolygonPilots_AngryBarbarianVoicePack_v1 LITE";

        // Asset paths and URLs
        private const string AssetStoreUrl = "https://assetstore.unity.com/packages/slug/421378";
        private const string DiscordUrl = "https://discord.gg/GvpgGrCDds";
        private const string DemoScenePath = "Assets/PolygonPilots/SFX/Angry Fantasy Warrior - Voice Pack Lite/Demo/Scenes/AngryBarbarianLite_DemoScene.unity";

        // Relative path to the PDF documentation file
        private const string DocumentationRelativePath = "Assets/PolygonPilots/SFX/Angry Fantasy Warrior - Voice Pack Lite/Documents/Asset Documentation & Technical Guide.pdf";

        static WelcomeWindow()
        {
            EditorApplication.update += ShowOnFirstImport;
        }

        private static void ShowOnFirstImport()
        {
            EditorApplication.update -= ShowOnFirstImport;

            if (!EditorPrefs.GetBool(PrefKeyShown, false))
            {
                EditorPrefs.SetBool(PrefKeyShown, true);
                OpenWindow();
            }
        }

        [MenuItem("Tools/Polygon Pilots/AngryBarbarianVoicePack/Welcome & Support")]
        public static void OpenWindow()
        {
            WelcomeWindow window = GetWindow<WelcomeWindow>(true, "Welcome!", true);
            window.minSize = new Vector2(380, 420);
            window.maxSize = new Vector2(380, 420);
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(15);

            // Header title
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Thank you for choosing this asset!", headerStyle);

            GUILayout.Space(10);

            // Intro message
            GUIStyle textStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(15, 15, 0, 0)
            };
            GUILayout.Label("I hope it fits smoothly into your project workflow. Below you will find essential documentation, links, and community support:", textStyle);

            GUILayout.Space(15);

            // Action buttons
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(15);
                using (new EditorGUILayout.VerticalScope())
                {
                    if (GUILayout.Button("📖 Open PDF Documentation", GUILayout.Height(32)))
                    {
                        OpenPdfDocumentation();
                    }

                    GUILayout.Space(6);

                    if (GUILayout.Button("🎮 Open Demo Scene", GUILayout.Height(32)))
                    {
                        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(DemoScenePath);
                        if (sceneAsset != null)
                        {
                            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(DemoScenePath);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Error", "Scene not found at path:\n" + DemoScenePath, "OK");
                        }
                    }

                    GUILayout.Space(6);

                    if (GUILayout.Button("💬 Community & Support (Discord)", GUILayout.Height(32)))
                    {
                        Application.OpenURL(DiscordUrl);
                    }

                    GUILayout.Space(18);

                    // Highlighted review button
                    GUI.backgroundColor = new Color(0.35f, 0.75f, 1.0f);
                    if (GUILayout.Button("⭐ Leave a Review on Asset Store", GUILayout.Height(36)))
                    {
                        Application.OpenURL(AssetStoreUrl);
                    }
                    GUI.backgroundColor = Color.white;
                }
                GUILayout.Space(15);
            }

            GUILayout.FlexibleSpace();

            // Footer note
            GUIStyle noteStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                wordWrap = true
            };
            GUILayout.Label("You can reopen this window anytime via:\nTools -> Polygon Pilots -> AngryBarbarianVoicePack -> Welcome & Support", noteStyle);
            GUILayout.Space(10);
        }

        private static void OpenPdfDocumentation()
        {
            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            string fullPdfPath = Path.Combine(projectPath, DocumentationRelativePath);

            if (File.Exists(fullPdfPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(fullPdfPath)
                {
                    UseShellExecute = true
                });
            }
            else
            {
                var docAsset = AssetDatabase.LoadAssetAtPath<Object>(DocumentationRelativePath);
                if (docAsset != null)
                {
                    EditorGUIUtility.PingObject(docAsset);
                    Selection.activeObject = docAsset;
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Documentation file not found at path:\n" + DocumentationRelativePath, "OK");
                }
            }
        }
    }
}