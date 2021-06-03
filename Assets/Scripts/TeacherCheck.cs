using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherCheck : MonoBehaviour
{
    public GameScript gameScript;
    private int stack;

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

    public void CallFakeRandom()
    {
        StartCoroutine(nameof(FakeRandom));
    }

    private IEnumerator FakeRandom()
    {
        var range = UnityEngine.Random.Range(1, 3);
        yield return new WaitForSeconds(range);
        gameScript.teacherAnimator.SetBool(gameScript.fake, false);
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
            gameScript.teacherAnimator.SetBool(gameScript.fake, true);
        }
        else if (range >= 46 || stack >= 5)
        {
            gameScript.teacherAnimator.SetBool(gameScript.watch, true);
            yield return new WaitForSeconds(0.5f);

            var turnRange = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(turnRange);
            gameScript.teacherAnimator.SetBool(gameScript.watch, false);
            StartCoroutine(nameof(TurnBack));
            stack = 0;
        }
        else
        {
            stack++;
            Debug.Log(stack);
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(nameof(Random));
        }
    }
}