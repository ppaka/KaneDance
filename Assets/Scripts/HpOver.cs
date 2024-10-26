using System.Collections;

#if UNITY_ANDROID
using GooglePlayGames;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HpOver : MonoBehaviour
{
    public AudioClip igonan;
    public CanvasGroup canvasGroup;
    public AudioSource audioSorry;
    public TMP_Text scoreTxt;
    public TMP_Text highScoreTxt;
    public TMP_Text successTxt;
    public GameObject boringKane;

    public Button skipTxt;
    
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

        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        var score = PlayerPrefs.GetInt("Score");
        scoreTxt.text = score.ToString();
        highScoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreTxt.text = "최고 기록!";
        }
#if UNITY_ANDROID
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ReportScore(PlayerPrefs.GetInt("HighScore"), GPGSIds.leaderboard_ranks, success =>
            {
                successTxt.text = success ? "" : "점수 등록 실패";
            });
                
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_first_try, 100, _ => {});
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_300, (double)score / 300*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_500, (double)score / 500*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_750, (double)score/750*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_1000, (double)score/1000*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_1500, (double)score/1500*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_2000, (double)score/2000*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_2500, (double)score/2500*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_3000, (double)score/3000*100, _ => { });
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_all_clear, (double)score/3000*100, _ => { });
        }
        else
        {
            successTxt.text = "소셜 로그인이 되어있지 않습니다!";
        }
#endif
    }
    
    public void SkipScreen()
    {
        StopCoroutine(nameof(ChangeScreen));
        
        skipTxt.gameObject.SetActive(false);
        
        boringKane.gameObject.SetActive(false);
        Camera.main!.backgroundColor = Color.white;
        audioSorry.clip = igonan;
        audioSorry.Play();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    private IEnumerator ChangeScreen()
    {
        yield return new WaitForSeconds(3);
        
        skipTxt.gameObject.SetActive(false);
        
        boringKane.gameObject.SetActive(false);
        Camera.main!.backgroundColor = Color.white;
        audioSorry.clip = igonan;
        audioSorry.Play();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}