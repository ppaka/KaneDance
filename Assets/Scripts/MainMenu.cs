using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionTxt;
    public TMP_Text score;
    public Image tagiriImg;
    public Sprite dmca, original;
    public Button exitButton;

    private void Start()
    {
        if (PlayerPrefs.GetInt("isDMCA", 1) == 0)
        {
            tagiriImg.sprite = original;
        }
        else if (PlayerPrefs.GetInt("isDMCA", 1) == 1)
        {
            tagiriImg.sprite = dmca;
        }

#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
#endif
        
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        
        versionTxt.text = Application.version;
        score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void ChangeSong()
    {
        if (PlayerPrefs.GetInt("isDMCA", 1) == 0)
        {
            tagiriImg.sprite = dmca;
            PlayerPrefs.SetInt("isDMCA", 1);
        }
        else if (PlayerPrefs.GetInt("isDMCA", 1) == 1)
        {
            tagiriImg.sprite = original;
            PlayerPrefs.SetInt("isDMCA", 0);
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
    
    public void LinkTgd()
    {
        Application.OpenURL("https://tgd.kr/s/kanetv8");
    }
    
    public void LinkTwitch()
    {
        Application.OpenURL("https://www.twitch.tv/kanetv8");
    }

    public void LinkGithub()
    {
        Application.OpenURL("https://github.com/ppaka/KaneDance");
    }
}