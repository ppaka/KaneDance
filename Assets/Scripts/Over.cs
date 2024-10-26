using System.Collections;

#if UNITY_ANDROID
using GooglePlayGames;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Over : MonoBehaviour
{
    public AudioClip ja, igonan;
    public SpriteRenderer fgetout, getout, sorry, hit;
    public Animator hittingAnim;
    public AudioSource audioSorry;
    public CanvasGroup canvasGroup;
    public TMP_Text scoreTxt;
    public TMP_Text highScoreTxt;
    public TMP_Text successTxt;

    public Button skipTxt;
    
    private static readonly int Start1 = Animator.StringToHash("Start");

    public void Retry()
    {
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
        var score = StaticVariables.recentScore;
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
        
        audioSorry.Stop();
        fgetout.gameObject.SetActive(false);
        getout.gameObject.SetActive(false);
        sorry.gameObject.SetActive(false);
        hit.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(2);
        
        fgetout.gameObject.SetActive(false);
        getout.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        Camera.main!.backgroundColor = new Color(0.7372549f, 0.8352942f, 0.6235294f);
        getout.gameObject.SetActive(false);
        sorry.gameObject.SetActive(true);
        audioSorry.Play();

        yield return new WaitForSeconds(2);

        audioSorry.Stop();
        Camera.main.backgroundColor = Color.white;
        sorry.gameObject.SetActive(false);
        hit.gameObject.SetActive(true);
        hittingAnim.SetTrigger(Start1);
        audioSorry.clip = ja;
        audioSorry.Play();

        yield return new WaitForSeconds(2);
        
        skipTxt.gameObject.SetActive(false);
        
        hit.gameObject.SetActive(false);
        audioSorry.clip = igonan;
        audioSorry.Play();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}