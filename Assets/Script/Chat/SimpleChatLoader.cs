using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SimpleChatLoader
{
    public struct StoryCharacter
    {
        public string CharacterID;
        public string Root;
    }

    public struct ChatActionBox
    {
        public ArrayList ActionList;
        public ArrayList CharacterList;
    }


    //读取故事文件
    public ChatActionBox LoadStory(string text)
    {
        ChatActionBox storylist = new ChatActionBox();

        string str1 = Regex.Unescape(text);
        str1 = str1.Replace("\r", "");
        string[] txt = Regex.Split(str1, "\n");

        storylist = ParseActionList(txt);
        
        return storylist;
    }

    //解析故事文件
    ChatActionBox ParseActionList(string[] list)
    {
        ChatActionBox box = new ChatActionBox();
        box.ActionList = new ArrayList();
        box.CharacterList = new ArrayList();

        //读取故事的类型，如果为0则读取类型为角色模式，如果为1则读取类型为故事模式
        int loadType = 0;

        foreach (string str in list)
        {
            //如果碰见注释符号或为空行，则忽略本行
            if (str.Contains("//") || str == "")
                continue;

            //设置读取类型
            if (str == "[Character]")
            {
                loadType = 0;
                continue;
            }
            else if (str == "[ChatList]")
            {
                loadType = 1;
                continue;
            }

            //读取角色模式的方法
            if (loadType == 0)
            {
                StoryCharacter character = new StoryCharacter();
                character.CharacterID = str.Substring(0, str.IndexOf("<"));

                string tempstr = str.Substring(str.IndexOf("<") + 1, str.IndexOf(">") - character.CharacterID.Length - 1);
                string[] parameter = tempstr.Split(';');

                for (int i = 0; i < parameter.Length; i++)
                {
                    //读取name
                    if (i == 0)
                        character.Root = parameter[i].Substring(5, parameter[i].Length - 5);
                }

                box.CharacterList.Add(character);
            }
            //读取故事模式的方法
            else if (loadType == 1)
            {
                string[] talks = str.Split(':');
                if (talks.Length < 2)
                    talks = str.Split('：');

                box.ActionList.Add(talks);
            }
        }
        return box;
    }
}
