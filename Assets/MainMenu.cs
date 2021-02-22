#if UNITY_STANDALONE_WIN
using Squirrel;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionTxt;
    public TMP_Text score;

    public Image prog_image;
    public TMP_Text btn_text;

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

        if (errorDetected == true && Task.Run(CheckForUpdate).Result == false)
        {
            btn_text.text = "업데이트를 하는 도중 오류가 발생했습니다";
        }
        else if (Task.Run(CheckForUpdate).Result == false)
        {
            btn_text.text = "최신버전 입니다";
        }
    }

    private string url = "https://github.com/ppaka/KaneDance";
    private bool updatePending;
    private bool errorDetected;

    private async Task<bool> CheckForUpdate()
    {
        try
        {
            using (var manager = await UpdateManager.GitHubUpdateManager(url))
            {
                var info = await manager.CheckForUpdate();

                if (info.ReleasesToApply.Count == 0)
                {
                    if (updatePending)
                    {
                        btn_text.text = "재시작하여 업데이트를 완료 해주세요!";
                        return true;
                    }

                    return false;
                }

                prog_image.fillAmount = 0;
                btn_text.text = "업데이트 다운로드중...";

                await manager.DownloadReleases(info.ReleasesToApply, p => prog_image.fillAmount = p / 100f);

                prog_image.fillAmount = 0;
                btn_text.text = "업데이트 설치중...";

                await manager.ApplyReleases(info, p => prog_image.fillAmount = p / 100f);

                updatePending = true;
                prog_image.fillAmount = 0;
                btn_text.text = "재시작하여 업데이트를 완료 해주세요!";
                errorDetected = false;
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            errorDetected = true;
            return false;
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
}