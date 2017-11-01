using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ElementsMenu : Editor
{
    [MenuItem("关卡工具/创建/创建点击工具 &1")]
    [MenuItem("GameObject/关卡工具/点击工具", priority = 0)]
    private static void ClickElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            GameObject obj = new GameObject();
            Image img = obj.AddComponent<Image>();
            obj.AddComponent<ClickElement>();
            obj.name = "ClickElement";
            obj.transform.SetParent(t);

            img.color = new Color((float)0 /255,(float)170/255, (float)0 /255);
            SetSize(obj);
            SetPos(obj);
        }
    }

    [MenuItem("关卡工具/创建/创建拖拽工具 &2")]
    [MenuItem("GameObject/关卡工具/拖拽工具", priority = 0)]
    private static void DragElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            GameObject obj = new GameObject();
            Image img = obj.AddComponent<Image>();
            obj.AddComponent<DragElement>();
            obj.name = "DragElement";
            obj.transform.SetParent(t);

            img.color = new Color((float)180 / 255, (float)0 / 255, (float)210 / 255);
            SetSize(obj);
            SetPos(obj);
        }
    }

    [MenuItem("关卡工具/创建/创建检查区域 &3")]
    [MenuItem("GameObject/关卡工具/检查区域", priority = 0)]
    private static void EventAreaOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            GameObject obj = new GameObject();
            Image img = obj.AddComponent<Image>();
            obj.AddComponent<EventArea>();
            obj.name = "EventArea";
            obj.transform.SetParent(t);

            img.color = new Color((float)255 / 255, (float)0 / 255, (float)0 / 255);
            SetSize(obj);
            SetPos(obj);
        }
    }

    [MenuItem("关卡工具/创建/创建道具工具 &4")]
    [MenuItem("GameObject/关卡工具/道具工具", priority = 0)]
    private static void ItemElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            GameObject obj = new GameObject();
            Image img = obj.AddComponent<Image>();
            obj.AddComponent<ItemElement>();
            obj.name = "ItemElement";
            obj.transform.SetParent(t);

            img.color = new Color((float)255 / 255, (float)140 / 255, (float)0 / 255);
            SetSize(obj);
            SetPos(obj);
        }
    }

    [MenuItem("关卡工具/添加/添加点击工具 #&1")]
    private static void AttachClickElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            if (t.GetComponent<ClickElement>() == null)
                t.gameObject.AddComponent<ClickElement>();
        }
    }

    [MenuItem("关卡工具/添加/添加拖拽工具 #&2")]
    private static void AttachDragElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            if(t.GetComponent<DragElement>() == null)
                t.gameObject.AddComponent<DragElement>();
        }
    }

    [MenuItem("关卡工具/添加/添加检查区域 #&3")]
    private static void AttachEventAreaOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            if (t.GetComponent<EventArea>() == null)
                t.gameObject.AddComponent<EventArea>();
        }
    }

    [MenuItem("关卡工具/添加/添加道具工具 #&4")]
    private static void AttachItemElementOption()
    {
        Transform[] transforms = Selection.transforms;
        //将选中的对象的postion保存在字典中
        foreach (Transform t in transforms)
        {
            if (t.GetComponent<ItemElement>() == null)
                t.gameObject.AddComponent<ItemElement>();
        }
    }

    private static void SetSize(GameObject obj)
    {
        RectTransform rt = obj.transform as RectTransform;
        rt.sizeDelta = new Vector2(100, 100);
        rt.localScale = new Vector3(1, 1, 1);
    }

    private static void SetPos(GameObject obj)
    {
        int ranint = 100;
        obj.transform.localPosition = new Vector3(Random.Range(-ranint, ranint), Random.Range(-ranint, ranint), 0);
    }
}
