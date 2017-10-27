using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElement : MonoBehaviour {
    LevelManager levelManager;
    [System.Serializable]
    public struct StateDo
    {
        public int StateID;
        public AnimationClip animation;
        public StateAction action;
    }

    public enum StateAction
    {
        None,
        Next,
        Complete,
        Fail
    }

    [Header("状态列表：")]
    public StateDo[] DoList;
    //[Space(10)]
    [Header("动画控制器：")]
    public Animator ani;

    // Use this for initialization
    virtual public void Awake () {
        IntiElement();
    }

    void Start()
    {
        EventTriggerListener.Get(transform).onClick = CheckOnClick;
    }

	// Update is called once per frame
	void Update () {
		
	}

    //初始化当前状态
    void IntiElement()
    {
        //获取关卡管理器
        levelManager = transform.Find("/Main Camera").GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.Log("获取不到关卡管理器! 物件名称为：" + transform);
            return;
        }
        if(DoList.Length==0)
        {
            Debug.Log("没有设置状态！物件名称为：" + transform);
            return;
        }

        //获取动画播放器
        if (ani == null)
        {
            ani = transform.GetComponent<Animator>();
            if (ani == null)
            {
                Debug.Log("获取不到动画播放器! 物件名称为：" + transform);
                return;
            }
        }

        Debug.LogFormat("物件<color=blue> {0} </color>初始化完成！", transform.name);
    }

    private void PlayAnimation()
    {
        ani.Play(GetComponent<Animation>().name);
    }

    private void CheckOnClick(GameObject go)
    {
        //如果正在播放动画则不响应点击
        if (GetLevelManager().isPlayingAnimation()) return;

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

    //确定是否下一步
    public void CheckAction(StateDo _do)
    {
        if (_do.action == StateAction.Next)
        {
            levelManager.AddNowState();
        }
        else if (_do.action == StateAction.Complete)
        {
            levelManager.CompleteLevel();
        }
        else if (_do.action == StateAction.Fail)
        {
            levelManager.FailLevel();
        }
    }

    public StateDo GetStateDo(int stateID)
    {
        StateDo common = new StateDo();

        //查找对应ID的动作
        foreach (StateDo _s in DoList)
        {
            if (_s.StateID == 0)
                common = _s;
            if (_s.StateID == stateID)
                return _s;
        }
        //如果找到默认动作，则返回默认动作，否则返回空
        if (common.StateID == 0)
            return common;
        else
        {
            Debug.LogFormat("找不到对应<color=red> {0} </color>的动作！", stateID);
            return common;
        }
    }

    public LevelManager GetLevelManager()
    {
        return levelManager;
    }
}
