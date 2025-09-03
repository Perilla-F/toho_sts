using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIDataEditor : Editor
{
    private SerializedProperty conditionsProp;

    private void OnEnable()
    {
        conditionsProp = serializedObject.FindProperty("conditionDatas");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("EnemyID"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("ConditionDatas", EditorStyles.boldLabel);

        for (int i = 0; i < conditionsProp.arraySize; i++)
        {
            SerializedProperty cond = conditionsProp.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = cond.FindPropertyRelative("ConditionName");
            SerializedProperty typeProp = cond.FindPropertyRelative("ConditionType");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            nameProp.stringValue = EditorGUILayout.TextField("Condition Name", nameProp.stringValue);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                conditionsProp.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(typeProp);

            EnemyConditionType type = (EnemyConditionType)typeProp.enumValueIndex;

            switch (type)
            {
                case EnemyConditionType.HP:
                    EditorGUILayout.PropertyField(cond.FindPropertyRelative("HpThresholdMin"));
                    EditorGUILayout.PropertyField(cond.FindPropertyRelative("HpThresholdMax"));
                    break;
                case EnemyConditionType.Turn:
                    EditorGUILayout.PropertyField(cond.FindPropertyRelative("MaxTurn"));
                    break;
                case EnemyConditionType.PlayerEffect:
                    EditorGUILayout.PropertyField(cond.FindPropertyRelative("RequiredPlayerEffect"));
                    break;
            }

            EditorGUILayout.PropertyField(cond.FindPropertyRelative("Actions"), true);
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Condition"))
        {
            conditionsProp.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
