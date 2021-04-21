using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Animator teacherAnimator;
    public Animator moongtaengAnimator;
    public Animator movingAnimator;
    public Animator moeAnimator;
    public Animator sorryAnimator;
    public new AudioSource audio;
    public TMP_Text scoreTxt;
    public Image hpBar;
    
    public AudioClip dmcaClip, originalClip;

    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");

    private float _score;
    private bool _keydown;
    private bool _gameOver;
    private bool _teacherWatching = true;
    private static readonly int Watch = Animator.StringToHash("Watch");

    private void Awake()
    {
        _keydown = false;
        StartCoroutine(nameof(FirstStart));
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("isDMCA", 1) == 0)
        {
            audio.clip = originalClip;
        }
        else if (PlayerPrefs.GetInt("isDMCA", 1) == 1)
        {
            audio.clip = dmcaClip;
        }
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        scoreTxt.text = _score.ToString("0");

        if (hpBar.fillAmount == 0)
        {
            PlayerPrefs.SetInt("Score", int.Parse(_score.ToString("0")));
            SceneManager.LoadScene("GameOver");
        }

        if (_keydown)
        {
            hpBar.fillAmount += Time.deltaTime * 0.10f;
            _score += Time.deltaTime * 1.5f * 15;
        }
        else
        {
            hpBar.fillAmount -= Time.deltaTime * 0.05f;
            _score += Time.deltaTime * 15;
        }

        if (_teacherWatching && _keydown)
        {
            PlayerPrefs.SetInt("Score", int.Parse(_score.ToString("0")));
            SceneManager.LoadScene("GameOver");
        }
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            try
            {
                if (_teacherWatching)
                {
                    PlayerPrefs.SetInt("Score", int.Parse(_score.ToString("0")));
                    SceneManager.LoadScene("GameOver");
                }
                
                _keydown = true;
                audio.Play();
                moongtaengAnimator.SetBool(SpaceDown, true);
                movingAnimator.SetBool(SpaceDown, true);
                moeAnimator.SetBool(SpaceDown, true);
                sorryAnimator.SetBool(SpaceDown, true);
            }
            catch
            {
                //
            }
        }
        if (ctx.canceled)
        {
            try
            {
                _keydown = false;
                audio.Stop();
                moongtaengAnimator.SetBool(SpaceDown, false);
                movingAnimator.SetBool(SpaceDown, false);
                moeAnimator.SetBool(SpaceDown, false);
                sorryAnimator.SetBool(SpaceDown, false);
            }
            catch
            {
                //
            }
        }
    }

    private IEnumerator FirstStart()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(nameof(TurnBack));
    }

    private IEnumerator TurnBack()
    {
        teacherAnimator.SetBool(Watch, false);
        yield return new WaitForSeconds(0.2f);
        _teacherWatching = false;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(nameof(Random));
    }

    private IEnumerator Random()
    {
        var range = UnityEngine.Random.Range(1, 100);
        if (range >= 78)
        {
            teacherAnimator.SetBool(Watch, true);
            yield return new WaitForSeconds(0.39f);
            _teacherWatching = true;
            
            var turnRange = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(turnRange);
            StartCoroutine(nameof(TurnBack));
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
            StartCoroutine(nameof(Random));
        }
    }
}
