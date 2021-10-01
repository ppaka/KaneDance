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
    public AudioSource audioSource;
    public TMP_Text scoreTxt;
    public Image hpBar;
    
    public AudioClip dmcaClip, originalClip;

    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");

    private float _score;
    private bool _keydown;
    private bool _gameOver;
    public bool teacherWatching = true;
    public readonly int Watch = Animator.StringToHash("Watch");

    private void Awake()
    {
        _keydown = false;
        StartCoroutine(nameof(FirstStart));
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("isDMCA", 1) == 0)
        {
            audioSource.clip = originalClip;
        }
        else if (PlayerPrefs.GetInt("isDMCA", 1) == 1)
        {
            audioSource.clip = dmcaClip;
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
        
        if (teacherWatching && _keydown)
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
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            try
            {
                if (teacherWatching)
                {
                    PlayerPrefs.SetInt("Score", int.Parse(_score.ToString("0")));
                    SceneManager.LoadScene("GameOver");
                }
                
                audioSource.Play();
                moongtaengAnimator.SetBool(SpaceDown, true);
                movingAnimator.SetBool(SpaceDown, true);
                moeAnimator.SetBool(SpaceDown, true);
                sorryAnimator.SetBool(SpaceDown, true);
                _keydown = true;
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
                audioSource.Stop();
                moongtaengAnimator.SetBool(SpaceDown, false);
                movingAnimator.SetBool(SpaceDown, false);
                moeAnimator.SetBool(SpaceDown, false);
                sorryAnimator.SetBool(SpaceDown, false);
                _keydown = false;
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
        teacherAnimator.SetBool(Watch, false);
    }
}
