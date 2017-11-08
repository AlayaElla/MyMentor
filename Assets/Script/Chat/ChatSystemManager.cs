using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatSystemManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    ChatSystemTool chatmanager;
    ArrayList StoryList = new ArrayList();

    public void StartStory()
    {
        if (StoryList.Count == 0 || StoryList == null)
        {
            Debug.LogError("Can't Start Story,Don't have StoryList!");
            return;
        }

        if (chatmanager == null)
        {
            GameObject newobj = new GameObject();
            newobj.name = "ChatSystem";
            chatmanager = newobj.AddComponent<ChatSystemTool>();
            newobj.AddComponent<AudioSource>();
        }
        chatmanager.PushStoryList(StoryList);
        string storyname = (string)StoryList[0];

        Loading.GetInstance().LoadingStoryScene(storyname, () =>
        {
            chatmanager.LoadChatStory(storyname);
        });
    }

    public void StartStory(string storyname)
    {
        AddStroyName(storyname);
        StartStory();
    }

    void AddStroyName(string storyname)
    {
        if(!StoryList.Contains(storyname))
            StoryList.Add(storyname);
    }
}
