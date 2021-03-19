using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.SocialPlatforms;

#endif
// ReSharper disable StringLiteralTypo

public class GoogleAccount : MonoBehaviour
{
    public static GoogleAccount Instance;

    private bool _waitingForAuth;
    public Button linkGoogle;
    public Button unlinkGoogle;
    public Button showLeaderboard;
    public Button showAchievement;

    public TMP_Text score;

#if UNITY_ANDROID
    private void Awake()
    {
        try
        {
            Instance = this;
            InitializeGooglePlayGamesService();
            _waitingForAuth = true;
            linkGoogle.gameObject.SetActive(true);
        }
        catch
        {
            linkGoogle.gameObject.SetActive(false);
        }

        if (Social.localUser.authenticated)
        {
            _waitingForAuth = false;
            unlinkGoogle.gameObject.SetActive(true);
            showLeaderboard.gameObject.SetActive(true);
            showAchievement.gameObject.SetActive(true);
            linkGoogle.gameObject.SetActive(false);
        }
    }

    private void InitializeGooglePlayGamesService()
    {
        var config = new PlayGamesClientConfiguration.Builder()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private static ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }

    public void LoadCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("save",DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood,
            (status, metadata) =>
            {
                if (status == SavedGameRequestStatus.Success)
                    SavedGame().ReadBinaryData(metadata, (requestStatus, bytes) =>
                    {
                        if (status == SavedGameRequestStatus.Success)
                        {
                            PlayerPrefs.SetInt("HighScore", int.Parse(bytes.ToString()));
                            
                            score.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
                        }
                    });
            });
    }

    public void SaveCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("save",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, (status, metadata) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    var update = new SavedGameMetadataUpdate.Builder().Build();
                    var bytes = System.Text.Encoding.UTF8.GetBytes(PlayerPrefs.GetInt("HighScore").ToString());
                    SavedGame().CommitUpdate(metadata, update, bytes, (requestStatus, gameMetadata) =>
                    {
                        if (status == SavedGameRequestStatus.Success)
                        {
                            Debug.Log("게임 저장 성공");
                        }
                        else Debug.Log("게임 저장 실패");
                    });
                }
            });
    }
    
#endif

    public void Login()
    {
        if (!_waitingForAuth) return;

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(result =>
                {
                    if (result)
                    {
                        _waitingForAuth = false;
                        unlinkGoogle.gameObject.SetActive(true);
                        showLeaderboard.gameObject.SetActive(true);
                        showAchievement.gameObject.SetActive(true);
                        linkGoogle.gameObject.SetActive(false);
                    }
                    else
                    {
                        _waitingForAuth = true;
                        unlinkGoogle.gameObject.SetActive(false);
                        showLeaderboard.gameObject.SetActive(false);
                        showAchievement.gameObject.SetActive(false);
                        linkGoogle.gameObject.SetActive(true);
                    }
                }
            );
        }
    }

    public void LogOut()
    {
        if (_waitingForAuth) return;

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.SignOut();
        _waitingForAuth = true;
        unlinkGoogle.gameObject.SetActive(false);
        showLeaderboard.gameObject.SetActive(false);
        showAchievement.gameObject.SetActive(false);
        linkGoogle.gameObject.SetActive(true);
#endif
    }

    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }

    public void Leaderboards()
    {
        Social.ShowLeaderboardUI();
    }
}