using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Over : MonoBehaviour
{
    public AudioClip ja, igonan;
    public Camera _camera;
    public SpriteRenderer getout, sorry, hit;
    public Animator hittingAnim;
    public AudioSource audioSorry;
    public CanvasGroup CanvasGroup;
    public TMP_Text scoreTxt;
    public TMP_Text highScoreTxt;

    private int score;

    public void Retry()
    {
        PlayerPrefs.DeleteKey("Score");
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main");
    }

    private void Start()
    {
        StartCoroutine(nameof(ChangeScreen));

        score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = score.ToString();
        highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreTxt.text = "최고 기록!";
        }
    }

    private IEnumerator ChangeScreen()
    {
        yield return new WaitForSeconds(2);

        _camera.backgroundColor = new Color(0.7372549f, 0.8352942f, 0.6235294f);
        getout.gameObject.SetActive(false);
        sorry.gameObject.SetActive(true);
        audioSorry.Play();

        yield return new WaitForSeconds(2);

        audioSorry.Stop();
        _camera.backgroundColor = Color.white;
        sorry.gameObject.SetActive(false);
        hit.gameObject.SetActive(true);
        hittingAnim.SetTrigger("Start");
        audioSorry.clip = ja;
        audioSorry.Play();

        yield return new WaitForSeconds(2);
        hit.gameObject.SetActive(false);
        audioSorry.clip = igonan;
        audioSorry.Play();
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }
}