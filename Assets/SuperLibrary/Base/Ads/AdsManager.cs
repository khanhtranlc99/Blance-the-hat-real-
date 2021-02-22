using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if EXISTED_IRON_SOURCE
using AppsFlyerSDK;
using FalconSDK;
#endif

public class AdsManager : MonoBehaviour
{
 
    [Header("Main Network")]
    [SerializeField]
    private AdMobile adNetWork = AdMobile.ADMOD;
    public static AdMobile AdNetwork { get => instance.adNetWork; }

    [Header("Options")]
    [SerializeField]
    private bool initAtStart = true;
    [SerializeField]
    protected bool useBanner = true;
    public static bool UseBanner => (bool)instance?.useBanner;
    public static bool AutoRewardToInter => (bool)DataManager.GameConfig?.autoRewardToInter;
    public static bool AutoInterToReward => (bool)DataManager.GameConfig?.autoInterToReward;

    public static bool IsDebugMode => DebugMode.IsDebugMode;
    [SerializeField]
    protected Button interstitialButton = null;
    [SerializeField]
    protected Button videoRewarded = null;

    [Header("Banner Safe Area")]
    [SerializeField]
    protected RectTransform parrentTransform;
    [SerializeField]
    protected float bannerHeight = 68f;
    [SerializeField]
    protected BannerPos bannerPos = BannerPos.BOTTOM;
    [SerializeField]
    protected GameObject bannerBackground = null;

    public static float TotalTimePlay { get; set; }
    public static int TotalSuccess { get; private set; }
    public static DateTime LastTimeShowAd { get; private set; }

    private static UserData userData => DataManager.UserData;
    private static GameConfig gameConfig => DataManager.GameConfig;

    [SerializeField]
    protected UIToggle isRemoveAds;
     
    public static bool IsRemoveAds
    {
        get
        {
            if (instance != null && userData != null)
            {
                if (instance.isRemoveAds && !userData.isRemovedAds)
                    userData.isRemovedAds = instance.isRemoveAds.isOn;
                return userData.isRemovedAds;
            }
            return false;
        }
    }

    /// <summary>
    /// Check time play more than game config to show ads
    /// </summary>
    public static bool IsTimeToShowAds
    {
        get
        {
            if (userData != null && gameConfig != null)
            {
                if (IsRemoveAds)
                {
                    Debug.Log(" isRemovedAds: " + userData.isRemovedAds);
                    return false;
                }

                
                if (LastTimeShowAd.AddSeconds(gameConfig.timePlayToShowAds) < DateTime.Now)
                {
                    //if (GameStateManager.CurrentState == GameState.Complete)
                    //    return true;
                    //if (Mathf.FloorToInt(TotalTimePlay) >= timePlayToShowAds)
                    //    return true;
                    return true;
                }
                else
                {
                    Debug.Log("Not time to show Ads!");
                    return false;
                }
            }
            return false;
        }
    }

    public delegate void AdsDelegate(AdType currentType, AdEvent currentEvent, string currentPlacement, string currentItemId);
    public static AdsDelegate OnStateChanged;

    private static AdsManager instance;

    public static AdType currentType { get; private set; }
    public static AdEvent currentEvent { get; private set; }
    public static string currentPlacement { get; private set; }
    public static string currentItem { get; private set; }

    public static bool InterstitialIsReady
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                return true;
            }

#if !USE_MAX && !USE_IRON && !USE_ADMOB && !USE_UNITY && !EXISTED_IRON_SOURCE
            return true;
#else
            if (AdNetwork == AdMobile.ADMOD)
            {
                return false;
            }
            else if (AdNetwork == AdMobile.IRONSOURCE)
            {
                return IronHelper.InterIsReady;
            }
            else if (AdNetwork == AdMobile.MAX)
            {
                return false;
            }
            else
                return false;
#endif
        }
    }

    public static bool VideoRewaredIsReady
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            {
                return true;
            }

#if !USE_MAX && !USE_IRON && !USE_ADMOB && !USE_UNITY && !EXISTED_IRON_SOURCE
            return true;
#else
            if (AdNetwork == AdMobile.ADMOD)
            {
                return false;
            }
            else if (AdNetwork == AdMobile.IRONSOURCE)
            {
                return IronHelper.RewardIsReady;
            }
            else if (AdNetwork == AdMobile.MAX)
            {
                return false;
            }
            else
                return false;
#endif
        }
    }

    private void Awake()
    {
        instance = this;

        if (isRemoveAds)
        {
            isRemoveAds.OnChangedAction((isOn) =>
            {
                UpdateBannerArea();
            });
        }

        DataManager.OnLoaded += DataManager_OnLoaded;
    }

    private void DataManager_OnLoaded(GameData gameData)
    {
        if (!initAtStart)
            Init();
        UpdateBannerArea();
    }

    private void Start()
    {
        interstitialButton?.onClick.AddListener(TestInterstitial);
        videoRewarded?.onClick.AddListener(TestVideoReward);

        if (initAtStart)
            Init();
    }

    public static void Init()
    {
        if (!instance)
        {
            Debug.LogError("[AdsManager] NULL");
            return;
        }

        if (AdNetwork == AdMobile.ADMOD)
        {
            Debug.LogError("Admob.Init(); Not implement!!");
        }
        else if (AdNetwork == AdMobile.IRONSOURCE)
        {
            IronHelper.Init(IsDebugMode);
        }
        else if (AdNetwork == AdMobile.MAX)
        {
            Debug.LogError("MaxHelper.Init(); Not implement!!");
        }
    }

    public void TestInterstitial()
    {
        LastTimeShowAd = new DateTime(1999, 1, 1);
        TotalTimePlay = 9999;
        ShowInterstitial((s) =>
        {
            Debug.Log("Test Interstitial: " + s);
        }, "TestInterstitialName", "TestInterstitialId");
    }
    /// <summary>
    /// Befor call should show loading then check status to do something. Flow game monetization on Gameover
    /// <para/>
    /// A.on RESULT SCREEN: 
    /// if (timePlayInGame >= timePlayToShowAds)
    /// {
    ///     ShowInterstitial((status) =>
    ///     {
    ///         if (status == AdEvent.Success)
    ///         {
    ///             resetTimePlayInGame();
    ///             do something
    ///         }
    ///         else
    ///         {
    ///             do something
    ///         }
    ///     }
    ///}
    ///<para/>
    ///B.on CONTINUE SCREEN: 
    ///if (userClickRebornByAds) 
    ///     => reset timePlayInGame
    /// else 
    ///     => flow on RESULT SCREEN
    /// </summary>
    public static void ShowInterstitial(Action<AdEvent> onSuccess, string placementName, string itemId)
    {
        if (!instance)
        {
            Debug.LogError("[AdsManager] NULL");
            onSuccess(AdEvent.NotAvailable);
            return;
        }

        if (!IsTimeToShowAds)
        {
            onSuccess(AdEvent.NotTimeToShow);
            return;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (string.IsNullOrEmpty(placementName))
                Debug.LogWarning("placementName IsNullOrEmpty");
            onSuccess(AdEvent.Success);
            return;
        }



#if !USE_MAX && !USE_IRON && !USE_ADMOB && !USE_UNITY && !EXISTED_IRON_SOURCE
        onSuccess(AdEvent.Success);
#else

        if (InterstitialIsReady)
        {
            ShowNoticeOnLoading();
        }
        else
        {
            onSuccess(AdEvent.NotAvailable);
            SetStatus(AdType.Interstitial, AdEvent.NotAvailable, placementName, itemId);
            return;
        }

        if (AdNetwork == AdMobile.ADMOD)
        {
            Debug.LogError("AdmobInterstitial.Show(onSuccess, placementName); Not implement!!");
        }
        else if (AdNetwork == AdMobile.IRONSOURCE)
        {
            IronHelper.ShowInterstitial(onSuccess, placementName, itemId);
        }
        else if (AdNetwork == AdMobile.MAX)
        {
            Debug.LogError("MaxInterstitial.Show(onSuccess, placementName); Not implement!!");
        }
        else
        {
            onSuccess?.Invoke(AdEvent.NotAvailable);
        }
#endif
    }

    public void TestVideoReward()
    {
        LastTimeShowAd = new DateTime(1999, 1, 1);
        TotalTimePlay = 9999;
        ShowVideoReward((s) =>
        {
            Debug.Log("Test VideoReward: " + s);
        }, "TestVideoRewardName", "TestVideoRewardId");
    }

    /// <summary>
    /// Befor call should show loading then check status to do something. Flow game monetization on Gameover
    /// <para/>
    /// LOGIC:
    /// ShowVideoReward((status) =>
    /// {
    ///     if (status == AdEvent.Success)
    ///     {
    ///         resetTimePlayInGame();
    ///         do something
    ///     }
    ///     else
    ///     {
    ///        do something
    ///     }
    /// }
    /// </summary>
    public static void ShowVideoReward(Action<AdEvent> onSuccess, string placementName = "", string itemId = "")
    {
        if (!instance)
        {
            Debug.LogError("[AdsManager] NULL");
            onSuccess(AdEvent.NotAvailable);
            return;
        }

        if (!IsConnected)
        {
            PopupMes.Show("Connection Error", "Failed to connect to server. Please check your internet connection and try again!", "Okie");
            SetStatus(AdType.VideoReward, AdEvent.NotInternet, placementName, itemId);
            onSuccess(AdEvent.NotInternet);
            return;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (string.IsNullOrEmpty(placementName))
                Debug.LogWarning("placementName IsNullOrEmpty");
            onSuccess(AdEvent.Success);
            return;
        }

        ShowNoticeOnLoading();

#if !USE_MAX && !USE_IRON && !USE_ADMOB && !USE_UNITY && !EXISTED_IRON_SOURCE
        onSuccess(AdEvent.Success);
#else
        if (AdNetwork == AdMobile.ADMOD)
        {
            Debug.LogError("AdmobVideoReward.Show(onSuccess, placement);  Not implement!!");
        }
        else if (AdNetwork == AdMobile.IRONSOURCE)
        {
            IronHelper.ShowRewarded(onSuccess, placementName, itemId);
        }
        else if (AdNetwork == AdMobile.MAX)
        {
            Debug.LogError("MaxVideoReward.Show(onSuccess, placement);  Not implement!!");
        }
        else
        {
            onSuccess?.Invoke(AdEvent.NotAvailable);
        }
#endif
    }

    public static void DestroyBanner()
    {
        if (AdNetwork == AdMobile.ADMOD)
        {
            Debug.LogError("Admob DestroyBanner  Not implement!!");
        }
        else if (AdNetwork == AdMobile.IRONSOURCE)
        {
#if USE_IRON
            IronHelper.DestroyBaner();
#endif
        }
        else if (AdNetwork == AdMobile.MAX)
        {
            Debug.LogError("Max DestroyBanner  Not implement!!");
        }
    }

    public static void SetStatus(AdType adType, AdEvent adEvent, string placementName = "", string itemId = "")
    {
        if (instance)
        {
            currentType = adType;
            currentEvent = adEvent;
            currentPlacement = placementName;
            currentItem = itemId;

            if (adEvent == AdEvent.Success)
            {
                TotalTimePlay = 0;
                LastTimeShowAd = DateTime.Now;
                TotalSuccess++;
            }

            GameStateManager.isBusy = adEvent == AdEvent.Show;
            OnStateChanged?.Invoke(currentType, currentEvent, currentPlacement, currentItem);

            DebugMode.Log(currentType.ToString() + " " + currentEvent.ToString() + " " + currentPlacement + " " + currentItem);
        }
    }

    public static bool IsConnected
    {
        get
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return true;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return true;
                default:
                    return false;
            }
        }
    }

    public static void ShowNoticeOnLoading()
    {
        try
        {
            UIToast.ShowLoading("Time to show ads... please wait!");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void ShowNoticeOnFail(AdEvent onSuccess)
    {
        try
        {
            if (onSuccess == AdEvent.NotInternet)
                UIToast.ShowError("Please check your internet connection...!");
            else if (onSuccess == AdEvent.NotAvailable)
                UIToast.ShowError("Video not ready, please try again...!");
            else if (onSuccess == AdEvent.Fail)
                UIToast.ShowError("Something wrong, please try again...!");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void UpdateBannerArea()
    {
        if (IsRemoveAds)
            DestroyBanner();
    }

    private void OnApplicationPause(bool isPaused)
    {
#if USE_IRON
        IronSource.Agent.onApplicationPause(isPaused);
#endif
    }

#if EXISTED_IRON_SOURCE
    private const string AppsflyerVideoAdsTrackKey = "VIDEO";
    private const string AppsflyerInterstitialAdsTrackKey = "FULLSCREEN";
    protected static Guid sessionId;
    protected static AdsInformation adsInfo;

    public static void LogFalconAds(AdsType adsType, AdsStatus adsStatus, string placementName, string itemId)
    {
        try
        {
            if (string.IsNullOrEmpty(placementName))
                placementName = "Unknow";
            if (string.IsNullOrEmpty(itemId))
                itemId = "Unknow";

            var where = placementName + "_" + itemId;

            var level = DataManager.CurrentStage != null ? DataManager.CurrentStage.index : 0;
            if (adsStatus == AdsStatus.Open)
            {
                adsInfo = MediationInfo.GetAvailableAdsInformation();

                if (adsInfo != null)
                    LogManager.LogAds(adsType, adsInfo.Id ?? "", adsInfo.Name ?? "", adsInfo.Name ?? "", where, level);
                else
                    LogManager.LogAds(adsType, "", "", "", where, level);

                if (adsType == AdsType.Interstitial)
                    AdsAppsFlyerOpenEventTracking(AppsflyerInterstitialAdsTrackKey);
                else
                    AdsAppsFlyerOpenEventTracking(AppsflyerVideoAdsTrackKey);
            }
            else if (adsStatus == AdsStatus.Success)
            {
                LogManager.UpdateLogAds(AdsStatus.Success);
                if (adsType == AdsType.Interstitial)
                    AdsAppsFlyerCompleteEventTracking(AppsflyerInterstitialAdsTrackKey);
                else
                    AdsAppsFlyerCompleteEventTracking(AppsflyerVideoAdsTrackKey);
            }
            else if (adsStatus == AdsStatus.Fail)
            {
                LogManager.UpdateLogAds(AdsStatus.Fail);
            }
            Debug.Log("LogFalconAds: " + sessionId.ToString() + " " + adsType + " " + adsStatus);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void AdsAppsFlyerOpenEventTracking(string ad_type)
    {
        var adWatchedEvent = new Dictionary<string, string>();
        adWatchedEvent.Add("af_type", ad_type);

        AppsFlyer.sendEvent("af_ad_opened_" + ad_type.ToLower(), adWatchedEvent);
    }

    private static void AdsAppsFlyerCompleteEventTracking(string ad_type)
    {
        var adWatchedEvent = new Dictionary<string, string>();
        adWatchedEvent.Add("af_type", ad_type);

        AppsFlyer.sendEvent("af_ad_complete_" + ad_type.ToLower(), adWatchedEvent);
    }
#endif
}


public enum BannerPos
{
    NONE,
    TOP,
    BOTTOM
}
