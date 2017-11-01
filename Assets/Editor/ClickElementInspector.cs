using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClickElement))]
public class ClickElementInspector : Editor
{
    private static GUIContent insertContent = new GUIContent("+  添加状态", "添加新的状态到状态列表"), deleteContent = new GUIContent("删除", "删除当前状态"), insertAniContent = new GUIContent("+", "添加新的动画到动画列表"), deleteAniContent = new GUIContent("-", "删除动画"), pointContent = GUIContent.none;
    private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(40f);

    //ClickElement element;
    private SerializedObject element;
    private SerializedProperty dolist;

    private bool[] showActionList;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        element = new SerializedObject(target);
        dolist = element.FindProperty("DoList");
        showActionList = new bool[dolist.arraySize];
    }

    //自定义检视面板
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("需要点击的物件使用的工具，物件点击效果依照状态列表中的配置来触发。\n状态ID为0的话，则是默认状态，即如果没有找到对应的状态ID就会触发这个状态中的动作。", MessageType.Info);
        element.Update();

        //状态列表标题
        GUILayout.Label("状态列表: ", EditorStyles.largeLabel);

        //状态列表内容
        for (int i = 0; i < dolist.arraySize; i++)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            SerializedProperty statedo = dolist.GetArrayElementAtIndex(i);
            GUILayout.Label("  状态ID");
            EditorGUILayout.PropertyField(statedo.FindPropertyRelative("StateID"), pointContent);
            //EditorGUILayout.PropertyField(action.FindPropertyRelative("ActionList"), pointContent);
            GUILayout.Label("     执行命令");
            EditorGUILayout.PropertyField(statedo.FindPropertyRelative("NextDo"), pointContent);

            if (GUILayout.Button(deleteContent, EditorStyles.miniButton, buttonWidth))
            {
                dolist.DeleteArrayElementAtIndex(i);
                SaveProperties();
                ChangeshowActionList();
                return;
            }
            EditorGUILayout.EndHorizontal();

            //状态列表内的动画列表
            EditorGUI.indentLevel = 1;
            SerializedProperty actinlist = statedo.FindPropertyRelative("ActionList");
            showActionList[i] = EditorGUILayout.Foldout(showActionList[i], "  动画列表: " + actinlist.arraySize + "个", true);

            if (showActionList[i])
            {
                for (int j = 0; j < actinlist.arraySize; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    SerializedProperty clip = actinlist.GetArrayElementAtIndex(j);
                    EditorGUILayout.PropertyField(clip, pointContent);
                    if (GUILayout.Button(deleteAniContent, EditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        actinlist.DeleteArrayElementAtIndex(j);
                        SaveProperties();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(170));
                if (GUILayout.Button(insertAniContent, EditorStyles.miniButton))
                {
                    actinlist.InsertArrayElementAtIndex(actinlist.arraySize);
                    SaveProperties();
                    return;
                }
                GUILayout.Label("", GUILayout.Width(170));
                EditorGUILayout.EndHorizontal();
            }
            //EditorGUILayout.PropertyField(statedo.FindPropertyRelative("ActionList"), true);
            EditorGUILayout.EndVertical();
        }
        if (GUILayout.Button(insertContent))
        {
            dolist.InsertArrayElementAtIndex(dolist.arraySize);
            SaveProperties();
            ChangeshowActionList();
            return;
        }
        SaveProperties();
    }

    void SaveProperties()
    {
        element.ApplyModifiedProperties();
    }

    void ChangeshowActionList()
    {
        bool[] oldlist = showActionList;
        showActionList = new bool[dolist.arraySize];
        for (int i = 0; i < Mathf.Min(dolist.arraySize, oldlist.Length); i++)
        {
            showActionList[i] = oldlist[i];
        }
    }
}
