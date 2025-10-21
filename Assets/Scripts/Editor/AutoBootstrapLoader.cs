#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoBootstrapLoader
{
    static AutoBootstrapLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string bootstrapScene = "Assets/Scenes/Bootstrap.unity";

            if (!EditorSceneManager.GetActiveScene().path.Contains("Bootstrap"))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(bootstrapScene);
                }
            }
        }
    }
}
#endif
