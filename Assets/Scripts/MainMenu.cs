#if UNITY_STANDALONE_WIN
using Squirrel;
#endif
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionTxt;
    public TMP_Text score;

    public Image progImage;
    public TMP_Text updateText;

#if UNITY_ANDROID
    private void Start()
    {
        versionTxt.text = Application.version;
        score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
#endif

#if UNITY_STANDALONE_WIN

    private void Start()
    {
        versionTxt.text = Application.version;
        score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        Task.Run(CheckForUpdate);
    }

    private const string URL = @"https://github.com/ppaka/KaneDance";
    private bool _updatePending;

    private async Task CheckForUpdate()
    {
        try
        {
            using (var manager = await UpdateManager.GitHubUpdateManager(URL))
            {
                var info = await manager.CheckForUpdate();

                if (info.ReleasesToApply.Count == 0)
                {
                    if (_updatePending)
                    {
                        updateText.text = "재시작하여 업데이트를 완료 해주세요!";
                        return;
                    }

                    updateText.text = "최신버전 입니다.";
                    return;
                }

                progImage.fillAmount = 0;
                updateText.text = "업데이트 다운로드중...";

                await manager.DownloadReleases(info.ReleasesToApply, p => progImage.fillAmount = p / 100f);

                progImage.fillAmount = 0;
                updateText.text = "업데이트 설치중...";

                await manager.ApplyReleases(info, p => progImage.fillAmount = p / 100f);

                _updatePending = true;
                progImage.fillAmount = 0;
                updateText.text = "재시작하여 업데이트를 완료 해주세요!";
            }
        }
        catch
        {
            //
        }
    }
#endif

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
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