using System.Collections;
using UnityEngine;

public class TeacherCheck : MonoBehaviour
{
    private GameScript _gameScript;

    private void Start()
    {
        _gameScript = FindAnyObjectByType<GameScript>();
    }

    public void WatchStu()
    {
        _gameScript.teacherWatching = true;
    }

    public void NotWatchStu()
    {
        _gameScript.teacherWatching = false;
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
        var range = UnityEngine.Random.Range(1, 100f);
        if (range >= 81)
        {
            _gameScript.teacherAnimator.SetBool(GameScript.Watch, true);
            yield return new WaitForSeconds(0.5f);

            var turnRange = UnityEngine.Random.Range(1f, 5.5f);
            yield return new WaitForSeconds(turnRange);
            _gameScript.teacherAnimator.SetBool(GameScript.Watch, false);
        }
        else
        {
            yield return new WaitForSeconds(1.3f);
            StartCoroutine(nameof(Random));
        }
    }
}