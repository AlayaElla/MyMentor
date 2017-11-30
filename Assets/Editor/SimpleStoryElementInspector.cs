using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleStoryElement))]
public class SimpleStoryElementInspector : Editor
{
    private static GUIContent insertContent = new GUIContent("+  添加状态", "添加新的状态到状态列表"), deleteContent = new GUIContent("删除", "删除当前状态"), insertAniContent = new GUIContent("+", "添加新的字幕到字幕列表"), deleteAniContent = new GUIContent("-", "删除字幕"), pointContent = GUIContent.none;
    private static GUILayoutOption buttonWidth = GUILayout.MaxWidth(40f);

    //ClickElement element;
    private SerializedObject element;
    private SerializedProperty dolist;
    private Transform transform;

    private bool[] showActionList;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        element = new SerializedObject(target);
        dolist = element.FindProperty("DoList");
        SimpleStoryElement _target = (SimpleStoryElement)target;
        transform = _target.transform;
        showActionList = new bool[dolist.arraySize];
    }

    //自定义检视面板
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("播放故事脚本的工具。", MessageType.Info);
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
            if (statedo.FindPropertyRelative("NextDo").enumValueIndex == 2)
            {
                EditorGUILayout.PropertyField(element.FindProperty("jumpnum"), pointContent, GUILayout.Width(50));
                EditorGUILayout.Space();
            }
            else if (statedo.FindPropertyRelative("NextDo").enumValueIndex == 3)
            {
                EditorGUILayout.PropertyField(element.FindProperty("sceneName"), pointContent, GUILayout.Width(50));
                EditorGUILayout.Space();
            }

            if (GUILayout.Button(deleteContent, EditorStyles.miniButton, buttonWidth))
            {
                dolist.DeleteArrayElementAtIndex(i);
                SaveProperties();
                ChangeshowActionList();
                return;
            }
            EditorGUILayout.EndHorizontal();

            //状态列表内的字幕列表
            //导入文本
            TextAsset ta = EditorGUILayout.ObjectField("  导入文本", null, typeof(TextAsset), false) as TextAsset;
            SerializedProperty actinlist = statedo.FindPropertyRelative("talks");
            showActionList[i] = EditorGUILayout.Foldout(showActionList[i], "  字幕列表: " + actinlist.arraySize + "个", true);

            if (ta != null)
            {
                SimpleChatLoader loader = new SimpleChatLoader();
                SimpleChatLoader.ChatActionBox box = loader.LoadStory(ta.text);

                SetActionList(box, actinlist);

                loader = null;
                return;
            }
            if (showActionList[i])
            {
                for (int j = 0; j < actinlist.arraySize; j++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    SerializedProperty talks = actinlist.GetArrayElementAtIndex(j);
                    EditorGUILayout.PropertyField(talks.FindPropertyRelative("character"), pointContent, GUILayout.Width(120f));
                    talks.FindPropertyRelative("talkstring").stringValue = EditorGUILayout.TextArea(talks.FindPropertyRelative("talkstring").stringValue);
                    if (GUILayout.Button(deleteAniContent, EditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        actinlist.DeleteArrayElementAtIndex(j);
                        SaveProperties();
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                //添加动画按钮
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("");
                if (GUILayout.Button(insertAniContent, EditorStyles.miniButton, GUILayout.MinWidth(80f), GUILayout.MaxWidth(200f)))
                {
                    actinlist.InsertArrayElementAtIndex(actinlist.arraySize);
                    SaveProperties();
                    return;
                }
                GUILayout.Label("");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space();
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

    void SetActionList(SimpleChatLoader.ChatActionBox box, SerializedProperty actinlist)
    {
        Dictionary<string, Transform> Characters = new Dictionary<string, Transform>();
        foreach (SimpleChatLoader.StoryCharacter stc in box.CharacterList)
        {
            Transform t = transform.parent.Find(stc.Root);
            if (t == null && transform.parent.name.CompareTo(stc.Root) == 0)
                t = transform.parent;
            if (t == null)
            {
                EditorGUILayout.HelpBox("找不到 " + stc.Root + " ,请检查动画名称是否和物件对应!", MessageType.Error);
            }

            Characters.Add(stc.CharacterID, t);
        }

        actinlist.ClearArray();
        actinlist.arraySize = box.ActionList.Count;
        for (int j = 0; j < actinlist.arraySize; j++)
        {
            SerializedProperty talks = actinlist.GetArrayElementAtIndex(j);

            string[] text = (string[])box.ActionList[j];
            talks.FindPropertyRelative("character").objectReferenceValue = Characters[text[0]];
            talks.FindPropertyRelative("talkstring").stringValue = text[1];
        }

        Debug.Log("导入完成!");
        SaveProperties();
    }
}
