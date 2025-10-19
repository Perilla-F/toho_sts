using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EncounterData))]
public class EncounterDataEditor : Editor
{
    private void OnSceneGUI()
    {
        EncounterData data = (EncounterData)target;

        if (data.Enemies == null || data.Enemies.Count == 0) return;

        for (int i = 0; i < data.Enemies.Count; i++)
        {
            // uiPositions が足りなければ追加
            if (data.UIPositions.Count <= i)
                data.UIPositions.Add(Vector2.zero);

            Vector3 pos = new Vector3(data.UIPositions[i].x, data.UIPositions[i].y, 0);

            // ハンドル表示
            Vector3 newPos = Handles.PositionHandle(pos, Quaternion.identity);

            // 位置が変わったら反映
            if (newPos != pos)
            {
                Undo.RecordObject(data, "Move Enemy UI Position");
                data.UIPositions[i] = new Vector2(newPos.x, newPos.y);
                EditorUtility.SetDirty(data);
            }

            // ラベル表示（敵名）
            Handles.Label(newPos + Vector3.up * 0.3f, data.Enemies[i].name);
        }
    }
}
