using System;
using UnityEngine;
using static AdsManager;

public abstract class AdsBase<T> : MonoBehaviour where T : AdsBase<T>
{
    public static string TAG = "[ADS] ";
    public string sdkKey = "";

    [Header("Android")]
    [Tooltip("IronSource require AppKey, Admob require AppId, , Unity require GameId, Max not require")]
    [SerializeField]
    protected string appIdANDROID = "";
    [SerializeField]
    protected string interIdANDROID = "";
    [SerializeField]
    protected string rewardIdANDROID = "";

    [Header("iPhone")]
    [Tooltip("IronSource require AppKey, Admob require AppId, Unity require GameId, Max not require")]
    [SerializeField]
    protected string appIdIPHONE = "";
    [SerializeField]
    protected string interIdIPHONE = "";
    [SerializeField]
    protected string rewardIdIPHONE = "";


    protected static string AppKeyId = "UnSupport";
    protected static string InterUnitId = "";
    protected static string RewardUnitId = "";

    [Header("Interstitial")]
    [SerializeField]
    protected int interCountMax = 3;
    private string interPlacemenetName = "Default";
    private string interItemId = "Default";
    private Action<AdEvent> onInterShowSuccess = null;

    public static int InterCountTry = 0;
    public static bool InterIsReady { get; set; }

    [Header("VideoRewared")]
    [SerializeField]
    protected int rewardCountMax = 3;
    private string rewardPlacementName = "Default";
    private string rewardItemId = "Default";
    protected Action<AdEvent> onRewardShowSuccess = null;
    protected AdEvent rewardEvent = AdEvent.NotAvailable;

    [Header("Banner")]
    [SerializeField]
    protected int bannerCountMax = 3;
    public static int BannerCountTry = 0;

    public static int RewardCountTry = 0;
    public static bool RewardIsReady { get; set; }

    [HideInInspector]
    protected bool isInit = false;

    protected static T instance = null;

    public virtual void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AppKeyId = appIdANDROID;
            InterUnitId = interIdANDROID;
            RewardUnitId = rewardIdANDROID;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AppKeyId = appIdIPHONE;
            InterUnitId = interIdIPHONE;
            RewardUnitId = rewardIdIPHONE;
        }

        instance = (T)this;
    }

    #region VIDEO REWARED
    public abstract void RewardInit();

    public abstract void RewardLoad();

    public abstract void RewardOnReady();

    public virtual void RewardOnLoadFailed(string error, object obj)
    {
        SetStatus(AdType.VideoReward, AdEvent.Fail, rewardPlacementName, rewardItemId);

        RewardIsReady = false;
        PauseApp(false);
        onRewardShowSuccess?.Invoke(AdEvent.Fail);
        onRewardShowSuccess = null;

        if (RewardCountTry < rewardCountMax)
        {
            if (obj != null)
                Debug.LogError(TAG + "RewardOnLoadFailed " + error + " rewardIsReady: " + RewardIsReady + " " + obj.ToString() + " re-trying in 3 seconds count " + RewardCountTry + "/" + rewardCountMax);
            RewardCountTry++;
            Invoke("RewardLoad", 3);
        }
    }

    public virtual void RewardOnShowFailed(string error, object obj)
    {
        SetStatus(AdType.VideoReward, AdEvent.Fail, rewardPlacementName, rewardItemId);

        RewardIsReady = false;
        PauseApp(false);
        onRewardShowSuccess?.Invoke(AdEvent.Fail);
        onRewardShowSuccess = null;
    }

    public virtual void RewardOnShowSuscess(object obj)
    {
        if (obj != null)
            Debug.Log("ADS RewardOnShowSuscess: " + obj.ToString());
        rewardEvent = AdEvent.Success;
        SetStatus(AdType.VideoReward, AdEvent.Success, rewardPlacementName, rewardItemId);

#if UNITY_ANDROID
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("ADS RewardOnShowSuscess: " + onRewardShowSuccess.ToString());
            onRewardShowSuccess?.Invoke(AdEvent.Success);
            onRewardShowSuccess = null;
        });
#else
        //Iphone --> onRewardShowSuccess onClose Event
#endif
    }

    public virtual void RewardOnClick(object obj = null)
    {
        SetStatus(AdType.VideoReward, AdEvent.Click, rewardPlacementName, rewardItemId);
    }

    public virtual void RewardOnClose()
    {
#if UNITY_IPHONE
        if (rewardEvent == AdEvent.Success)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                onRewardShowSuccess?.Invoke(AdEvent.Success);
                onRewardShowSuccess = null;
            });   
        }
#else
        //Android --> onRewardShowSuccess onSuccess Event
#endif

        PauseApp(false);
        rewardEvent = AdEvent.Load;
        RewardLoad();
    }

    protected void RewardShow(Action<AdEvent> status, string placementName, string itemId)
    {
        rewardEvent = AdEvent.Show;
        InterCountTry = 0;
        rewardPlacementName = placementName;
        rewardItemId = itemId;
        onRewardShowSuccess = status;
        SetStatus(AdType.VideoReward, AdEvent.Show, rewardPlacementName, rewardItemId);
        PauseApp(true);
    }
    #endregion

    #region INTERSTITIAL
    public abstract void InterInit();

    public abstract void InterLoad();

    public abstract void InterOnReady();

    public virtual void InterOnLoadFailed(object obj = null)
    {
        SetStatus(AdType.Interstitial, AdEvent.Fail, interPlacemenetName, interItemId);
        InterIsReady = false;

        PauseApp(false);
        onInterShowSuccess?.Invoke(AdEvent.Fail);
        onInterShowSuccess = null;

        if (InterCountTry < interCountMax)
        {
            if (obj != null)
                Debug.LogError(TAG + "InterOnLoadFailed " + obj.ToString() + " re-trying in 3 seconds " + InterCountTry + "/" + interCountMax);
            InterCountTry++;
            Invoke("InterLoad", 3);
        }
    }

    public virtual void InterOnShowFailed(object obj = null)
    {
        SetStatus(AdType.Interstitial, AdEvent.Fail, interPlacemenetName, interItemId);

        InterIsReady = false;
        PauseApp(false);
        onInterShowSuccess?.Invoke(AdEvent.Fail);
        onInterShowSuccess = null;

        InterLoad();
    }

    public virtual void InterOnShowSuscess()
    {
        SetStatus(AdType.Interstitial, AdEvent.Success, interPlacemenetName, interItemId);
        onInterShowSuccess?.Invoke(AdEvent.Success);
        onInterShowSuccess = null;
    }

    public virtual void InterOnClick()
    {
        SetStatus(AdType.Interstitial, AdEvent.Click, interPlacemenetName, interItemId);
    }

    public virtual void InterOnClose()
    {
        SetStatus(AdType.Interstitial, AdEvent.Close, interPlacemenetName, interItemId);
        InterLoad();
        PauseApp(false);
    }

    protected void InterShow(Action<AdEvent> status, string placementName, string itemId)
    {
        onInterShowSuccess = status;
        interPlacemenetName = placementName;
        interItemId = itemId;
        SetStatus(AdType.Interstitial, AdEvent.Show, interPlacemenetName, interItemId);
        PauseApp(true);
    }

    private void PauseApp(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            MusicManager.AudioSourceReal.Pause();
        }
        else
        {
            Time.timeScale = 1;
            MusicManager.AudioSourceReal.UnPause();
        }
    }
    #endregion

    #region BANNER
    public abstract void InitBanner();

    public abstract void LoadBanner();
    #endregion
}
