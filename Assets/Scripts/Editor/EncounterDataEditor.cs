using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EncounterData))]
public class EncounterDataEditor : Editor
{
    private void OnSceneGUI()
    {
        EncounterData data = (EncounterData)target;

        if (data.enemies == null || data.enemies.Count == 0) return;

        for (int i = 0; i < data.enemies.Count; i++)
        {
            // uiPositions が足りなければ追加
            if (data.uiPositions.Count <= i)
                data.uiPositions.Add(Vector2.zero);

            Vector3 pos = new Vector3(data.uiPositions[i].x, data.uiPositions[i].y, 0);

            // ハンドル表示
            Vector3 newPos = Handles.PositionHandle(pos, Quaternion.identity);

            // 位置が変わったら反映
            if (newPos != pos)
            {
                Undo.RecordObject(data, "Move Enemy UI Position");
                data.uiPositions[i] = new Vector2(newPos.x, newPos.y);
                EditorUtility.SetDirty(data);
            }

            // ラベル表示（敵名）
            Handles.Label(newPos + Vector3.up * 0.3f, data.enemies[i].name);
        }
    }
}
