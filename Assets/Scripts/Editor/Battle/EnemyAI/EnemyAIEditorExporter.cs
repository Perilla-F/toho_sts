using UnityEditor;
using UnityEngine;
using System.IO;
using Serialization.EnemyAI;

public class EnemyAIEditorExporter : EditorWindow
{
    private EnemyAIEditorAsset selectedAsset;

    [MenuItem("Tools/Enemy AI Exporter")]
    public static void ShowWindow()
    {
        GetWindow<EnemyAIEditorExporter>("Enemy AI Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enemy AI JSON Exporter", EditorStyles.boldLabel);

        selectedAsset = (EnemyAIEditorAsset)EditorGUILayout.ObjectField("Enemy AI Asset", selectedAsset, typeof(EnemyAIEditorAsset), false);

        if (selectedAsset != null)
        {
            if (GUILayout.Button("Export to JSON"))
            {
                ExportToJson(selectedAsset);
            }
        }
    }

    private void ExportToJson(EnemyAIEditorAsset asset)
    {
        string path = EditorUtility.SaveFilePanel("Export Enemy AI to JSON", Application.dataPath, asset.enemyId, "json");
        if (string.IsNullOrEmpty(path)) return;

        var serializableData = EnemyAISerializer.ToSerializableData(asset);
        string json = JsonUtility.ToJson(serializableData, true);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();

        Debug.Log($"Exported EnemyAI to: {path}");
    }
}
