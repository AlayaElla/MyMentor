using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CheckArea : LevelElement, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform Area;
    private CanvasGroup group;
    public CheckType checkType;
    public enum CheckType
    {
        OnDrop,
        OnEnter,
        None
    }
    float prealpha = 1f;

    void Start()
    {
        IntiArea();
    }
	
    //初始化当前状态
    void IntiArea()
    {
        Area = transform as RectTransform;

        //创建画布组
        group = transform.GetComponent<CanvasGroup>();
        if (group == null)
            group = gameObject.AddComponent<CanvasGroup>();

        Debug.Log("chush");
    }

    public void OnDrop(PointerEventData data)
    {
        if (checkType != CheckType.OnDrop)
            return;

        MoveToCenter(data);
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (GetDropObject(data) == null)
            return;

        //变色效果
        if (checkType == CheckType.OnDrop)
        {
            prealpha = group.alpha;
            if (group.alpha > 0.5f)
                group.alpha = group.alpha / 2;
            else
                group.alpha = group.alpha * 2;
        }

        if (checkType != CheckType.OnEnter)
            return;

        MoveToCenter(data);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (GetDropObject(data) == null)
            return;

        //变色效果
        if (checkType == CheckType.OnDrop)
        {
            group.alpha = prealpha;
        }
    }

    private GameObject GetDropObject(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<DragPanel>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        var canvasgroup = originalObj.GetComponent<CanvasGroup>();
        if (canvasgroup == null)
            return null;

        return originalObj;
    }

    void MoveToCenter(PointerEventData data)
    {
        GameObject dropObject = GetDropObject(data);
        if (dropObject != null)
        {
            data.pointerDrag = null;
            dropObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            MoveToCenter effect = dropObject.AddComponent<MoveToCenter>();
            effect.SetPos(Area.position);

            CheckOnMoveIn();
        }
    }

    private void CheckOnMoveIn()
    {
        int stateID = GetLevelManager().GetNowState();
        StateDo _do = GetStateDo(stateID);
        if (_do.StateID == stateID || _do.StateID == 0)
        {
            //执行动作
            if (_do.animation != null)
            {
                ani.Play(_do.animation.name, 0, 0);
                GetLevelManager().SetLevelState(LevelManager.LevelStateType.PlayAnimation);

                //等待动画播放完执行下一步判断
                TimeTool.SetWaitTime(_do.animation.length, gameObject, () =>
                {
                    GetLevelManager().SetLevelState(LevelManager.LevelStateType.Common);
                    CheckAction(_do);
                });
            }
            else
            {
                CheckAction(_do);
            }
        }
    }
}
