using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
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

    private int _score;
    private static readonly int Start1 = Animator.StringToHash("Start");

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

        _score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = _score.ToString();
        highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();

        if (_score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _score);
            highScoreTxt.text = "최고 기록!";
#if UNITY_ANDROID
            if (Social.localUser.authenticated)
            {
                GoogleAccount.Instance.SaveCloud();
                Social.ReportProgress(GPGSIds.achievement_first_try, 100, success => {});
                Social.ReportScore(PlayerPrefs.GetInt("HighScore"), GPGSIds.leaderboard_ranks, success =>
                {
                    if (success)
                        successTxt.text = "점수 등록 성공!";
                    else
                        successTxt.text = "점수 등록 실패...";
                });
                
                Social.ReportProgress(GPGSIds.achievement_300, _score/300, success => { });
                Social.ReportProgress(GPGSIds.achievement_500, _score/500, success => { });
                Social.ReportProgress(GPGSIds.achievement_750, _score/750, success => { });
                Social.ReportProgress(GPGSIds.achievement_1000, _score/1000, success => { });
                Social.ReportProgress(GPGSIds.achievement_1500, _score/1500, success => { });
                Social.ReportProgress(GPGSIds.achievement_2000, _score/2000, success => { });
                Social.ReportProgress(GPGSIds.achievement_2500, _score/2500, success => { });
                Social.ReportProgress(GPGSIds.achievement_3000, _score/3000, success => { });
                Social.ReportProgress(GPGSIds.achievement_all_clear, _score/3000, success => { });
            }
            else
            {
                successTxt.text = "점수를 등록하려면 메인 메뉴에서 게임패드 모양의 버튼을 눌러주세요...";
            }
#endif
        }
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
        hittingAnim.SetTrigger(Start1);
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