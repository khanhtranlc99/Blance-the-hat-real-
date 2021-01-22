using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    #region Properties static
    private static UserData userData => DataManager.UserData;
    private static StageData currentStage => DataManager.CurrentStage;
    private static ItemData currentItem => DataManager.CurrentItem;

    public static string TAG
    {
        get
        {
            if (instance != null)
                return "[" + instance.GetType().Name + "] ";
            return "";
        }
    }

    private static AnalyticsManager instance { get; set; }
    #endregion

    protected void Awake()
    {
        instance = this;
        AdsManager.OnStateChanged += OnAdStateChanged;
        GameStateManager.OnStateChanged += GameStateManager_OnStateChanged;
    }

    private void GameStateManager_OnStateChanged(GameState current, GameState last, object data)
    {
        switch (current)
        {
            case GameState.Init:
                gameTimeStart = DateTime.Now;
                LogEvent($"game_mode_{currentStage.gameMode}");
                LogEvent($"using_item_{currentItem.id}");
                break;
            case GameState.GameOver:
                LogEvent($"lose_game_mode_{currentStage.gameMode}");
                LogEvent($"lose_item_{currentItem.id}");
                break;
            case GameState.Complete:
                var key = $"win_item_{currentItem.id}";
                var totalMatch = PlayerPrefs.GetInt(key, 0);
                if (totalMatch == 0)
                {
                    LogEvent($"{key}_first_win", logItem);
                }
                totalMatch++;
                PlayerPrefs.SetInt(key, totalMatch);

                LogEvent($"{key}");
                LogEvent($"win_game_mode_{currentStage.gameMode}");
                break;
        }
    }

    DateTime gameTimeStart = DateTime.Now;

    


    private void OnAdStateChanged(AdType currentType, AdEvent currentEvent, string currentPlacement, string currentItemId)
    {
        if (currentEvent == AdEvent.Success)
        {
            if (currentType == AdType.Banner)
                userData.TotalAdBanner++;
            else if (currentType == AdType.Interstitial)
                userData.TotalAdInterstitial++;
            else if (currentType == AdType.VideoReward)
                userData.TotalAdRewarded++;

            LogAdsViewed(currentType, currentPlacement, currentItemId);
        }
        else if(currentEvent == AdEvent.Show)
        {
            AnalyticsManager.LogAdsShow(AdType.VideoReward, currentPlacement, currentItemId);
        }
    }

    public static void LogInApp(string productId, InAppPurchaseEvent action, string failureReason = "")
    {
        if (userData != null)
        {
            if (action == InAppPurchaseEvent.Succeeded)
                userData.TotalPurchased++;

            var log = new Dictionary<string, object>
            {
                { "product_id", productId }
            };

            if (!string.IsNullOrEmpty(failureReason))
                log.Add("failure", failureReason);

            if (action == InAppPurchaseEvent.Succeeded || action == InAppPurchaseEvent.Failed)
            {
                LogEvent("in_app_" + action.ToString(), log);
                LogEvent("in_app_" + productId, log);
            }
        }
    }

    

    private void OnApplicationQuit()
    {
    }
    private void OnApplicationPause(bool pause)
    {
    }

    public static Dictionary<string, object> logUser
    {
        get
        {
            if (userData != null)
            {
                var log = new Dictionary<string, object>
                {
                    { "day", (DateTime.Now - userData.FistTimeOpen).Days },

                    { "total_play", userData.TotalPlay },
                    { "total_time_play", userData.TotalTimePlay },

                    { "total_ad_banner", userData.TotalAdBanner },
                    { "total_ad_interstitial", userData.TotalAdInterstitial },
                    { "total_ad_rewarded", userData.TotalAdRewarded },
                    { "total_in_app", userData.TotalPurchased }
                };
                return log;
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }
    }

    public static Dictionary<string, object> logItem
    {
        get
        {
            if (currentItem != null)
            {
                var log = new Dictionary<string, object>
                {
                    {"item",  currentItem.id},
                };
                return log;
            }
            else
            {
                return new Dictionary<string, object>();
            }
        }
    }


    public static void LogEvent(string eventName, Dictionary<string, object> eventData = null)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("eventName IsNullOrEmpty");
            return;
        }

        if (eventName.Length >= 32)
            eventName = eventName.Substring(0, 32);
        if (!eventName.Contains("_"))
            eventName = Regex.Replace(eventName, @"\B[A-Z]", m => "_" + m.ToString()).Replace("__", "_").ToLower();
        else
            eventName = eventName.ToLower();

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            string debugLog = eventName;
            if (eventData != null)
            {
                var entries = eventData.Select(d => string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
                debugLog += "\n" + "{" + string.Join(",", entries) + "}";
            }
            Debug.LogWarning(debugLog);

            return;
        }

        if (eventData == null)
        {
            Analytics.CustomEvent(eventName, null);
            FirebaseHelper.LogEvent(eventName, null);
        }
        else
        {
            Analytics.CustomEvent(eventName, eventData);
            FirebaseHelper.LogEvent(eventName, eventData);
        }
    }


    public static void LogAdsShow(AdType currentType, string placementName, string itemId)
    {
        if (string.IsNullOrEmpty(placementName))
            placementName = "Unknow";
        if (string.IsNullOrEmpty(itemId))
            itemId = "Unknow";

        var log = new Dictionary<string, object>();
        log.Add("source", $"{placementName}_{itemId}");
        LogEvent(currentType + "_show", log);
    }
    public static void LogAdsViewed(AdType currentType, string placementName, string itemId)
    {
        if (string.IsNullOrEmpty(placementName))
            placementName = "Unknow";
        if (string.IsNullOrEmpty(itemId))
            itemId = "Unknow";

        var log = new Dictionary<string, object>();
        log.Add("source", $"{placementName}_{itemId}");
        LogEvent(currentType + "_viewed", log);
    }

    public static void LogEarnMoney(MoneyType moneyType, string placementName, string itemId, int value)
    {
        if (string.IsNullOrEmpty(placementName))
            placementName = "Unknow";
        if (string.IsNullOrEmpty(itemId))
            itemId = "Unknow";

        var log = new Dictionary<string, object>();
        log.Add("source", $"{placementName}_{itemId}");
        log.Add("amount", value);

        if (moneyType == MoneyType.Gold)
            LogEvent("earn_gold", log);
        if (moneyType == MoneyType.Gem)
            LogEvent("earn_crystal", log);
    }

    public static void LogSpendMoney(MoneyType moneyType, string placementName, string itemId, int value)
    {
        if (string.IsNullOrEmpty(placementName))
            placementName = "Unknow";
        if (string.IsNullOrEmpty(itemId))
            itemId = "Unknow";

        var log = new Dictionary<string, object>();
        log.Add("source", $"{placementName}_{itemId}");
        log.Add("amount", value);

        if (moneyType == MoneyType.Gold)
            LogEvent("spend_gold", log);
        if (moneyType == MoneyType.Gem)
            LogEvent("spend_crystal", log);
    }
}

public enum MoneyType
{
    Gold,
    Gem
}