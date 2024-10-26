﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

#endif
// ReSharper disable StringLiteralTypo

public class GoogleAccount : MonoBehaviour
{
    private bool _waitingForAuth;
    public Button linkGoogle;
    public Button showLeaderboard;
    public Button showAchievement;

    public TMP_Text score;

#if UNITY_ANDROID
    private void Awake()
    {
        try
        {
            InitializeGooglePlayGamesService();
            _waitingForAuth = true;
            linkGoogle.gameObject.SetActive(true);
        }
        catch
        {
            linkGoogle.gameObject.SetActive(false);
        }

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            _waitingForAuth = false;
            showLeaderboard.gameObject.SetActive(true);
            showAchievement.gameObject.SetActive(true);
            linkGoogle.gameObject.SetActive(false);
        }
    }

    private void InitializeGooglePlayGamesService()
    {
        //var config = new PlayGamesClientConfiguration.Builder().Build();

        //PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            _waitingForAuth = false;
            showLeaderboard.gameObject.SetActive(true);
            showAchievement.gameObject.SetActive(true);
            linkGoogle.gameObject.SetActive(false);
        }
        else
        {
#if UNITY_ANDROID
            _waitingForAuth = true;
            showLeaderboard.gameObject.SetActive(false);
            showAchievement.gameObject.SetActive(false);
            linkGoogle.gameObject.SetActive(true);
#endif
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
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
#if UNITY_ANDROID
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.localUser.Authenticate(result =>
                {
                    if (result)
                    {
                        _waitingForAuth = false;
                        showLeaderboard.gameObject.SetActive(true);
                        showAchievement.gameObject.SetActive(true);
                        linkGoogle.gameObject.SetActive(false);
                    }
                    else
                    {
                        _waitingForAuth = true;
                        showLeaderboard.gameObject.SetActive(false);
                        showAchievement.gameObject.SetActive(false);
                        linkGoogle.gameObject.SetActive(true);
                    }
                }
            );
        }
#endif
    }

    public void Achievements()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
    }

    public void Leaderboards()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#endif
    }
}