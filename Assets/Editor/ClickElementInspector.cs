using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClickElement))]
public class ClickElementInspector : Editor
{

    private static GUIContent insertContent = new GUIContent("添加", "添加新的状态到状态列表"), deleteContent = new GUIContent("-", "删除当前状态"), pointContent = GUIContent.none;

    private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(30f), addWidth = GUILayout.MaxWidth(70f);

    //ClickElement element;
    private SerializedObject element;
    private SerializedProperty actionlist;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        element = new SerializedObject(target);
        actionlist = element.FindProperty("DoList");
    }

    //自定义检视面板
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("需要点击的物件使用的工具，物件点击效果依照状态列表中的配置来触发。\n", MessageType.Info);
        element.Update();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("状态列表:", GUILayout.Width(70));
        if (GUILayout.Button(insertContent, EditorStyles.miniButton, addWidth))
        {
            actionlist.InsertArrayElementAtIndex(actionlist.arraySize);
        }
        EditorGUILayout.EndHorizontal();
        for (int i = 0;
        i < actionlist.arraySize;
        i++)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            SerializedProperty action = actionlist.GetArrayElementAtIndex(i);
            GUILayout.Label("  状态ID", GUILayout.Width(50));
            EditorGUILayout.PropertyField(action.FindPropertyRelative("StateID"), pointContent, GUILayout.Width(40));
            //EditorGUILayout.PropertyField(action.FindPropertyRelative("ActionList"), pointContent);
            GUILayout.Label("     执行命令", GUILayout.Width(70));
            EditorGUILayout.PropertyField(action.FindPropertyRelative("NextDo"), pointContent, GUILayout.Width(80));

            if (GUILayout.Button(deleteContent, EditorStyles.miniButton, buttonWidth))
            {
                actionlist.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(action.FindPropertyRelative("ActionList"), true);
            EditorGUILayout.EndVertical();
        }
        element.ApplyModifiedProperties();
    }
}
