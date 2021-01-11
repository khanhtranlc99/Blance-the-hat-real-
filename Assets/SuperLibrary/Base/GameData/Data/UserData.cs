using System;
using System.Linq;
using UnityEngine;


[Serializable]
public class UserData : UserAnalysic
{
    [Header("Data")]
    public int live = 15;
    public int liveCountDown;
    public float latencyUser = 0;
    public int level = 1;

    private string lastTimeUpdate = new DateTime(1999, 1, 1).ToString();
    public DateTime LastTimeUpdate
    {
        get => DateTimeConverter.ToDateTime(lastTimeUpdate);
        set => lastTimeUpdate = value.ToString();
    }
    private string fistTimeOpen = DateTime.Now.ToString();
    public DateTime FistTimeOpen
    {
        get => DateTimeConverter.ToDateTime(fistTimeOpen);
        set => fistTimeOpen = value.ToString();
    }
    public int isRemovedAds;
    public double limitedPassTimeCountDown;

    [Header("Money")]
    [SerializeField]
    private int coin = 0;
    public int totalCoin
    {
        get => coin;
        set
        {
            if (coin < 100000)
            {
                if (coin != value)
                {
                    int changed = 0;
                    if (coin > value)
                    {
                        changed = coin - value;
                        totalCoinSpend += changed;
                    }
                    else
                    {
                        changed = value - coin;
                        totalCoinEarn += changed;
                    }

                    coin = value;
                    OnCoinChanged?.Invoke(changed, coin);
                }
            }
            else
            {
                UIToast.ShowError("Don't do that!");
                coin = 100;
                totalCoinEarn = 0;
                totalCoinSpend = 0;
            }
        }
    }
    public int totalCoinEarn = 0;
    public int totalCoinSpend = 0;

    [SerializeField]
    private int diamond;
    public int totalDiamond
    {
        get => diamond;
        set
        {
            if (diamond != value)
            {
                int changed = 0;
                if (diamond > value)
                {
                    changed = diamond - value;
                    totalDiamondSpend += changed;
                }
                else
                {
                    changed = value - diamond;
                    totalDiamondEarn += changed;
                }

                diamond = value;
                OnDiamondChanged?.Invoke(changed, diamond);
            }
        }
    }
    public int totalDiamondEarn = 0;
    public int totalDiamondSpend = 0;

    [SerializeField]
    private int star;
    public int totalStar
    {
        get
        {
            if (star == 0)
                star = DataManager.StagesAsset.list.Sum(x => x.star);
            return star;
        }
        set
        {
            if (star != value)
            {
                int changed = 0;
                if (star > value)
                {
                    changed = star - value;
                    totalStarSpend += changed;
                }
                else
                {
                    changed = value - star;
                    totalStarEarn += changed;
                }

                star = value;
                OnStarChanged?.Invoke(changed, star);
            }
        }
    }
    public int totalStarEarn = 0;
    public int totalStarSpend = 0;


    private int totalStageUnlocked = 1;
    public int TotalStageUnlocked
    {
        get
        {
            if (DataManager.StagesAsset != null && DataManager.StagesAsset.unlockedList != null)
            {
                var temp = DataManager.StagesAsset.unlockedList.Count;
                if (temp != totalStageUnlocked)
                {
                    totalStageUnlocked = temp;
                }
                return totalStageUnlocked;
            }
            return 1;
        }
    }

    private int totalPurchased = 0;
    public int TotalPurchased
    {
        get => totalPurchased;
        set
        {
            if (totalPurchased != value && value > 0)
            {
                totalPurchased = value;
            }
        }
    }

    public delegate void MoneyChangedDelegate(int changedValue, int current);
    public static event MoneyChangedDelegate OnCoinChanged;
    public static event MoneyChangedDelegate OnDiamondChanged;
    public static event MoneyChangedDelegate OnStarChanged;
}

[Serializable]
public class UserAnalysic : UserBase
{
    [Header("Analysic")]
    private int versionInstall;
    public int VersionInstall
    {
        get => versionInstall;
        set
        {
            if (versionInstall != value)
            {
                versionInstall = value;
            }
        }
    }

    private int versionCurrent;
    public int VersionCurrent
    {
        get => versionCurrent;
        set
        {
            if (versionCurrent != value)
            {
                versionCurrent = value;
            }
        }
    }

    private int session = 0;
    public int Session
    {
        get => session;
        set
        {
            if (session != value && value > 0)
            {
                session = value;
            }
        }
    }

    private long totalPlay = 0;
    public long TotalPlay
    {
        get => totalPlay;
        set
        {
            if (totalPlay != value && value > 0)
            {
                totalPlay = value;
            }
        }
    }

    private long totalTimePlay = 0;
    public long TotalTimePlay
    {
        get => totalTimePlay;
        set
        {
            if (totalTimePlay != value && value > 0)
            {
                totalTimePlay = value;
            }
        }
    }

    [Header("Ads")]
    private long totalAdInterstitial = 0;
    public long TotalAdInterstitial
    {
        get => totalAdInterstitial;
        set
        {
            if (totalAdInterstitial != value && value > 0)
            {
                totalAdInterstitial = value;
            }
        }
    }

    private long totalAdRewarded = 0;
    public long TotalAdRewarded
    {
        get => totalAdRewarded;
        set
        {
            if (totalAdRewarded != value && value > 0)
            {
                totalAdRewarded = value;
            }
        }
    }

    public long totalAdBanner = 0;
    public long TotalAdBanner
    {
        get => totalAdBanner;
        set
        {
            if (totalAdBanner != value && value > 0)
            {
                totalAdBanner = value;
            }
        }
    }

    private string abTesting;
    public string ABTesting
    {
        get
        {
            if (string.IsNullOrEmpty(abTesting))
            {
                int randonAB = UnityEngine.Random.Range(0, 3);
                if (randonAB == 0)
                    abTesting = "A";
                else if (randonAB == 1)
                    abTesting = "B";
                else
                    abTesting = "C";
            }
            return abTesting;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(abTesting))
            {
                abTesting = value;
            }
        }
    }

    private string source;
    public string Source
    {
        get => source;
        set
        {
            if (!string.IsNullOrEmpty(value) && source != value)
            {
                source = value;
            }
        }
    }
}

[Serializable]
public class UserBase
{
    [Header("Base")]
    public string id;
    public string email;
    public string name;
}