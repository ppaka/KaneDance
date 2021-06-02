using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherCheck : MonoBehaviour
{
    public GameScript gameScript;
    
    public void WatchStu()
    {
        gameScript.teacherWatching = true;
    }

    public void NotWatchStu()
    {
        gameScript.teacherWatching = false;
    }
}
