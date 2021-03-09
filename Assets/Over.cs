using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
// ReSharper disable StringLiteralTypo

public class Over : MonoBehaviour
{
    public AudioClip ja, igonan;
    public Camera _camera;
    public SpriteRenderer fgetout, getout, sorry, hit;
    public Animator hittingAnim;
    public AudioSource audioSorry;
    public CanvasGroup CanvasGroup;
    public TMP_Text scoreTxt;
    public TMP_Text highScoreTxt;
    public TMP_Text successTxt;

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
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ReportScore((long)PlayerPrefs.GetInt("HighScore"), "CgkI2dzCzbcSEAIQAQ",
                success =>
                {
                    if (success)
                    {
                        successTxt.text = "점수 등록 완료!";
                    }
                    else
                    {
                        successTxt.text = "점수 등록 실패...";
                    }
                });
        }
        else
        {
            successTxt.text = "점수를 등록하려면 메인 메뉴에서 게임패드 모양의 버튼을 눌러주세요...";
        }
#endif
    }

    private IEnumerator ChangeScreen()
    {
        yield return new WaitForSeconds(2);
        
        fgetout.gameObject.SetActive(false);
        getout.gameObject.SetActive(true);

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