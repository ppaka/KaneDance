using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Animator teacher_animator;
    public Animator moongtaeng_animator;
    public Animator moving_animator;
    public Animator moe_animator;
    public Animator sorry_animator;
    public new AudioSource audio;
    public TMP_Text scoreTxt;
    public Image hpBar;

    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");

    private float score;
    private bool keydown;
    private bool gameover;
    private bool teacherWatching;
    private static readonly int Watch = Animator.StringToHash("Watch");

    private void Awake()
    {
        keydown = false;
        StartCoroutine(nameof(FirstStart));
    }

    private void Update()
    {
        scoreTxt.text = score.ToString("0");

        if (hpBar.fillAmount == 0)
        {
            PlayerPrefs.SetInt("Score", int.Parse(score.ToString("0")));
            SceneManager.LoadScene("GameOver");
        }

        if (keydown)
        {
            hpBar.fillAmount += Time.deltaTime * 2 * 0.1f;
            score += (Time.deltaTime * 1.5f * 20);
        }
        else
        {
            hpBar.fillAmount -= Time.deltaTime * 0.07f;
            score += (Time.deltaTime * 20);
        }

        if (teacherWatching && keydown)
        {
            PlayerPrefs.SetInt("Score", int.Parse(score.ToString("0")));
            SceneManager.LoadScene("GameOver");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !keydown || Input.GetKeyDown(KeyCode.Mouse0) && !keydown)
        {
            keydown = true;
            audio.Play();
            moongtaeng_animator.SetBool(SpaceDown, true);
            moving_animator.SetBool(SpaceDown, true);
            moe_animator.SetBool(SpaceDown, true);
            sorry_animator.SetBool(SpaceDown, true);
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            keydown = false;

            audio.Stop();
            moongtaeng_animator.SetBool(SpaceDown, false);
            moving_animator.SetBool(SpaceDown, false);
            moe_animator.SetBool(SpaceDown, false);
            sorry_animator.SetBool(SpaceDown, false);
        }
    }

    private IEnumerator FirstStart()
    {
        teacherWatching = true;
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(nameof(TurnBack));
    }

    private IEnumerator TurnBack()
    {
        teacherWatching = false;
        teacher_animator.SetBool(Watch, false);
        StartCoroutine(nameof(Random));
        yield break;
    }

    private IEnumerator Random()
    {
        teacherWatching = false;

        var range = UnityEngine.Random.Range(1, 100);
        if (range >= 77)
        {
            teacher_animator.SetBool(Watch, true);
            yield return new WaitForSeconds(0.4f);
            teacherWatching = true;
            var turnbackRange = UnityEngine.Random.Range(1, 4);
            yield return new WaitForSeconds(turnbackRange);
            StartCoroutine(nameof(TurnBack));
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(nameof(Random));
        }
    }
}
