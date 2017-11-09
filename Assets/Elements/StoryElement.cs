using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryElement : ElementBase
{

    ChatSystemManager manager;

    // Use this for initialization
    override public void Awake()
    {
        IntiElement();
        manager = transform.Find("/Main Camera").GetComponent<ChatSystemManager>();
    }

    // Use this for initialization
    void Start () {
        manager.StartStory("shop1_2");

    }
	
}
