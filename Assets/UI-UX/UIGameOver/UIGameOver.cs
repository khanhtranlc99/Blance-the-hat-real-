using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : UIGameOverBase<UIGameOver>
{
    [Header("Stars")]
    [SerializeField]
    protected Image[] starImages = null;

    [Header("Counter")]
    [SerializeField]
    protected Text score = null;
    [SerializeField]
    protected string scoreFormat = "{0}";

    [SerializeField]
    protected string soundScoreFxCount = "sfx_score_count";
    [SerializeField]
    protected string soundScoreFxComplete = "sfx_score_complete";
    [SerializeField]
    protected string soundScoreFxBest = "sfx_score_best";

    [SerializeField]
    protected string rankFormat = "{0}";
    [Header("Coin Info")]
    [SerializeField]
    protected GameObject scaleCoinGroup = null;
    [SerializeField]
    protected Text coin = null;
    [SerializeField]
    protected Text scaleCoin = null;
    [SerializeField]
    protected int scaleCoinValue = 5;
    [SerializeField]
    protected Text scaleCoinDetail = null;
    [SerializeField]
    protected Button scaleCoinButton = null;

    [SerializeField]
    protected string soundFxCount = "";
    [SerializeField]
    protected string soundFxEnd = "";




    public void Show(GameState gameState)
    {
        Status = UIAnimStatus.IsAnimationShow;

        if (gameState == GameState.GameOver)
        {
            if (IsShowContinue)
            {
                ShowContinue();
            }
            else
            {
                ShowResult();
            }
        }
        else if (gameState == GameState.Complete)
        {
            ShowResult();
        }
    }

    public bool IsShowContinue
    {
        get
        {
            return rebornCount < rebornCountMax;
        }
    }

    public override void InitResultData()
    {
        base.InitResultData();

        stageName.text = /*DataManager.CurrentStage.name.ToUpper();*/"" ;

        if (starImages != null)
        {
            foreach (var i in starImages)
            {
                i.SetAlpha(0);
                i.transform.SetScale(2);
            }
        }

        if (score)
            score.text = "0";

        if (GameStatisticsManager.goldEarn < 5)
            GameStatisticsManager.goldEarn = 5 + (GameStatisticsManager.Stars * 5);

        else if (GameStatisticsManager.goldEarn <= 10)
            scaleCoinValue = 5;
        else if (GameStatisticsManager.goldEarn <= 15)
            scaleCoinValue = 3;
        else
            scaleCoinValue = 2;

        //coin.text = "0";
        //scaleCoin.text = "0";
        //scaleCoinDetail.text = "x" + scaleCoinValue;
        //scaleCoinGroup.SetActive(false);

        backButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        nextButton?.gameObject.SetActive(false);
    }

    public override void InitRebornData()
    {
        //Fill data then call show
        base.InitRebornData();
    }

    public override void OnShowResult()
    {
        SuggestionAds();
        ShowButton();
        DOStarsAnimation(() =>
        DOScoreAnimation(() =>
        DONumberAnimation(() =>
        {
            Status = UIAnimStatus.IsShow;
        })));
        
        CoinManager.Add(DataManager.GameConfig.coinPerStage);
    }

    public void ShowButton()
    {
        backButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        CheckScreenshot();
    }

    public void DOScoreAnimation(Action onDone = null, float animationTime = 0.5f, float delayTime = 0f)
    {
        if (score)
        {
            if (GameStatisticsManager.Score > 0)
            {
                score.DOText(0, GameStatisticsManager.Score, animationTime, delayTime, scoreFormat,
                    (s) =>
                    {
                        if (GameStatisticsManager.Score > 10)
                            SoundManager.Play(soundScoreFxCount);
                    },
                    () =>
                    {
                        if (GameStatisticsManager.Score > currentStage.score)
                        {
                            currentStage.score = GameStatisticsManager.Score;
                            SoundManager.Play(soundScoreFxBest);
                            stageSubName.text = "New RECORD";
                        }
                        else
                        {
                            SoundManager.Play(soundScoreFxComplete);
                            stageSubName.text = "Best score " + currentStage.score;
                        }
                        onDone?.Invoke();
                    });
            }
            else
            {
                score.text = "--";
                onDone?.Invoke();
            }
        }
    }

    private void DONumberAnimation(Action onDone)
    {
        int animationCount = 0;
        float animationTime = 0.25f;


        if (scaleCoinButton)
        {
            scaleCoinGroup.SetActive(true);
            animationCount++;
            int totalScaleCoin = GameStatisticsManager.goldEarn * scaleCoinValue;
            scaleCoinDetail.text = "x" + scaleCoinValue;
            scaleCoinButton.onClick.RemoveAllListeners();
            scaleCoinButton.onClick.AddListener(() =>
            {
            });

            coin.DOText(0, GameStatisticsManager.goldEarn, animationTime, animationTime * animationCount, rankFormat);
            scaleCoin.DOText(0, totalScaleCoin, animationTime, animationTime * animationCount, rankFormat,
                (s) =>
                {
                    if (totalScaleCoin > 0)
                        SoundManager.Play(soundFxCount);
                },
                () => SoundManager.Play(soundFxEnd));
        }

        DOVirtual.DelayedCall(animationCount * 0.25f, () =>
        {
            onDone?.Invoke();
        });
    }

    private void DOStarsAnimation(Action onDone = null)
    {
        if (starImages != null && GameStatisticsManager.Stars > 0)
        {
            for (int i = 0; i < starImages.Length; i++)
            {
                if (i < GameStatisticsManager.Stars)
                {
                    float timeAmin = 0.25f;
                    float timeDelay = 0.15f * i;
                    bool isLastStar = i == GameStatisticsManager.Stars - 1;
                    starImages[i].DOFadeIn(timeAmin, timeDelay, 0, 0.9f);
                    starImages[i].DOScale(timeAmin, 2, 1, timeDelay, () =>
                    {
                        if (isLastStar)
                            onDone?.Invoke();
                    });
                }
            }
        }
        else
        {
            onDone?.Invoke();
        }
    }

    #region Ads
    public void SuggestionAds()
    {
        StagesAsset datas = DataManager.StagesAsset;

        //Suggestion unlock by Star
        nextButton.onClick.RemoveAllListeners();

        var nextStage = datas.GetNext(UnlockType.Star, datas.Current.index, userData.totalStar);
        if (nextStage != null)
        {
            nextStage.isUnlocked = true;
            nextStage.unlockBy = UnlockType.Star;
            DataManager.Save();

            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(() => {
                DataManager.CurrentStage = nextStage;
                GameStateManager.Next(null);
                Hide(() =>
                {
                });
            });
        }
    }  

    private void OnShowVideoAdDone()
    {
        UIToast.ShowNotice("Thanks for watching video...!");
    }
}
#endregion
