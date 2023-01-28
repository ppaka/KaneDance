using System.Collections;
using UnityEngine;

public class TeacherCheck : MonoBehaviour
{
    public GameScript gameScript;

    private UnityRandom _uRand;
    private float _t;

    private void Start()
    {
        _uRand = new UnityRandom();
    }

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
        var range = _uRand.Range(1, 100, UnityRandom.Normalization.POWERLAW, _t);
        // range = UnityEngine.Random.Range(1, 100f);
        // print(range);
        if (range >= 81)
        {
            _t = 0;
            gameScript.teacherAnimator.SetBool(gameScript.Watch, true);
            yield return new WaitForSeconds(0.5f);

            var turnRange = UnityEngine.Random.Range(1f, 5.5f);
            yield return new WaitForSeconds(turnRange);
            gameScript.teacherAnimator.SetBool(gameScript.Watch, false);
        }
        else
        {
            _t += 0.02f;
            yield return new WaitForSeconds(1.3f);
            StartCoroutine(nameof(Random));
        }
    }
}