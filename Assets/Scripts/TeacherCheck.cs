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

    public void CallTurnBack()
    {
        StartCoroutine(nameof(TurnBack));
    }

    private IEnumerator TurnBack()
    {
        yield return new WaitForSeconds(0.6f);
        StartCoroutine(nameof(Random));
    }

    private IEnumerator Random()
    {
        var range = UnityEngine.Random.Range(1, 100);
        if (range >= 71)
        {
            gameScript.teacherAnimator.SetBool(gameScript.watch, true);
            yield return new WaitForSeconds(0.5f);

            var turnRange = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(turnRange);
            gameScript.teacherAnimator.SetBool(gameScript.watch, false);
            StartCoroutine(nameof(TurnBack));
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(nameof(Random));
        }
    }
}