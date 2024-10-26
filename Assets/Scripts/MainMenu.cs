using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionTxt;
    public TMP_Text score;
    public TMP_Text bgmModeText;
    public Button exitButton;

    private void Start()
    {
        Debug.Log($"Current Display RefreshRate {Screen.currentResolution.refreshRateRatio.value}");
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        }
        else
        {
            Application.targetFrameRate = 1000;
        }
        
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        versionTxt.text = Application.version;
        score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        
        var isDmca = PlayerPrefs.GetInt("isDMCA", 1);
        StaticVariables.isDmca = isDmca;
        switch (isDmca)
        {
            case 0:
                bgmModeText.SetText("음악 모드\n원본");
                break;
            case 1:
                bgmModeText.SetText("음악 모드\nDMCA 회피");
                break;
        }

#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
#endif
    }

    public void ChangeSong()
    {
        var dmcaValue = PlayerPrefs.GetInt("isDMCA", 1);
        if (dmcaValue == 0)
        {
            dmcaValue = 1;
        }
        else if (dmcaValue == 1)
        {
            dmcaValue = 0;
        }
        
        PlayerPrefs.SetInt("isDMCA", dmcaValue);
        StaticVariables.isDmca = dmcaValue;
        switch (dmcaValue)
        {
            case 0:
                bgmModeText.SetText("음악 모드\n원본");
                break;
            case 1:
                bgmModeText.SetText("음악 모드\nDMCA 회피");
                break;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void InfiniteMode()
    {
        SceneManager.LoadScene("InfiniteGame");
    }

    public void Exit()
    {
        PlayerPrefs.DeleteKey("Score");
        Application.Quit();
    }
    
    public void LinkSoop()
    {
        Application.OpenURL("https://ch.sooplive.co.kr/udkn");
    }
    
    public void LinkCafe()
    {
        Application.OpenURL("https://cafe.naver.com/kanetv");
    }

    public void LinkGithub()
    {
        Application.OpenURL("https://github.com/ppaka/KaneDance");
    }
}