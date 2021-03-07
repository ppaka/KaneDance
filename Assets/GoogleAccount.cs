using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAccount : MonoBehaviour
{
    private bool _waitingForAuth;
    public Button linkGpgs;
    public Button unlinkGpgs;
    public Button showLeaderboard;
    public Button showAchievement;
    
    private void Awake()
    {
        try
        {
            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            _waitingForAuth = true;
            linkGpgs.gameObject.SetActive(true);
        }
        catch
        {
            linkGpgs.gameObject.SetActive(false);
        }
    }

    public void Login()
    {
        if (!_waitingForAuth) return;
        
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(AuthenticateCallback);
        }
    }
    
    public void LogOut()
    {
        if (_waitingForAuth) return;
        
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
            _waitingForAuth = true;
            unlinkGpgs.gameObject.SetActive(false);
            showLeaderboard.gameObject.SetActive(false);
            showAchievement.gameObject.SetActive(false);
            linkGpgs.gameObject.SetActive(true);
        }
    }

    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }
    
    public void Leaderboards()
    {
        Social.ShowLeaderboardUI();
    }
    
    private void AuthenticateCallback(bool success)
    {
        if (success)
        {
            _waitingForAuth = false;
            unlinkGpgs.gameObject.SetActive(true);
            showLeaderboard.gameObject.SetActive(true);
            showAchievement.gameObject.SetActive(true);
            linkGpgs.gameObject.SetActive(false);
        }
        else
        {
            _waitingForAuth = true;
            unlinkGpgs.gameObject.SetActive(false);
            showLeaderboard.gameObject.SetActive(false);
            showAchievement.gameObject.SetActive(false);
            linkGpgs.gameObject.SetActive(true);
        }
    }
}