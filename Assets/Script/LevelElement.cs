using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Boundary
//{
//    public float xMin, xMax, zMin, zMax;
//}

public class LevelElement : MonoBehaviour {

    LevelManager levelManager;
    Animator ani;

    [System.Serializable]
    public struct StateDo
    {
        public bool complete;
        public bool next;
        public AnimationClip animation;
    }

    [Header("状态ID")]
    public StateDo[] DoList;

    // Use this for initialization
    void Start () {
        IntiElement();
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
        ani = transform.Find("/GameCanvas/bgLayer").GetComponent<Animator>();
        if (ani == null)
        {
            Debug.Log("获取不到动画播放器! 物件名称为：" + transform);
            return;
        }

        Debug.LogFormat("物件<color=blue> {0} </color>初始化完成！", transform.name);
    }

    void PlayAnimation()
    {
        ani.Play(GetComponent<Animation>().name);
    }

    void CheckOnClick(GameObject go)
    {
        int state = levelManager.GetNowState();

        if (DoList.Length - 1 >= state)
        {
            StateDo _do = DoList[state];
            if (_do.animation != null)
            {
                ani.Play(_do.animation.name,0,0);
            }
            if (_do.next)
            {
                levelManager.AddNowState();
            }
            if (_do.complete)
            {
                levelManager.CompleteLevel();
            }
        }
        else
        {
            Debug.LogFormat("找不到对应<color=red> {0} </color>的动作！", state);
            return;
        }
    }
}
