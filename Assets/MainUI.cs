using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public void LinkTgd()
    {
        Application.OpenURL("https://tgd.kr/s/kanetv8");
    }
    
    public void LinkTwitch()
    {
        Application.OpenURL("https://www.twitch.tv/kanetv8");
    }

    public void LinkGithub()
    {
        Application.OpenURL("https://github.com/ppaka/KaneDance");
    }
}
