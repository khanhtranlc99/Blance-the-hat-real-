using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIAnimation))]
public abstract class UIGameOverBase<T> : MonoBehaviour where T : UIGameOverBase<T>
{
    protected UserData userData => DataManager.UserData;
    protected GameConfig gameConfig => DataManager.GameConfig;
    protected StageData currentStage => DataManager.CurrentStage;

    [Header("Base")]
    [SerializeField]
    protected UIAnimation anim = null;
    public UIAnimStatus Status = UIAnimStatus.IsHide;

    [Header("Result")]
    [SerializeField]
    protected UIAnimation animResult = null;

    [Header("Stage Info")]
    [SerializeField]
    protected Text stageName;
    [SerializeField]
    protected Text stageSubName;

    [Header("Buttons Base")]
    [SerializeField]
    protected Button backButton = null;
    [SerializeField]
    protected Button restartButton = null;
    [SerializeField]
    protected Button nextButton = null;

    [Header("Share")]
    [SerializeField]
    protected Button shareButton = null;
    [SerializeField]
    protected Image shareImage = null;
    [SerializeField]
    protected string shareTitle = "Can you beat me!?";
    [SerializeField]
    protected string shareMessage = "FREE! Get it now...!";

    [Header("Continue")]
    [SerializeField]
    protected UIAnimation animContinue = null;

    [SerializeField]
    protected RebornType rebornType = RebornType.Continue;
    [SerializeField]
    protected RebornBy rebornBy = RebornBy.Ads;
    [SerializeField]
    protected int rebornCount = 0;
    [SerializeField]
    protected int rebornCountMax = 1;
    [SerializeField]
    protected Text rebornByTitle = null;
    [SerializeField]
    protected Text rebornByDes = null;
    [SerializeField]
    protected Text rebornByInfo = null;

    [Header("CountDown")]
    private float rebornElapsedTime = 0;
    [SerializeField]
    private int rebornElapsedMaxTime = 5;
    [SerializeField]
    protected Text rebornByCountDownText = null;
    [SerializeField]
    protected Image rebornByCountDownImage = null;
    [SerializeField]
    protected Transform rebornByCountDownImageDotTransform = null;
    [SerializeField]
    protected Button rebornBySkipButton = null;
    [SerializeField]
    protected Button noThanksButton = null;

    [Header("Continue Free")]
    [SerializeField]
    protected Button rebornByFreeButton = null;

    [Header("Continue Ads")]
    [SerializeField]
    protected Button rebornByAdsButton = null;

    [Header("Continue Coin")]
    [SerializeField]
    protected Button rebornByCoinButton = null;
    [SerializeField]
    protected Text rebornByCoinDes = null;
    [SerializeField]
    protected int rebornByCoinCost = 50;
    [SerializeField]
    protected int rebornByCoinTotalCost = 0;

    [Header("Continue Diamond")]
    [SerializeField]
    protected Button rebornByDiamondButton = null;
    [SerializeField]
    protected Text rebornByDiamondDes = null;
    [SerializeField]
    protected int rebornByDiamondCost = 10;
    [SerializeField]
    protected int rebornByDiamonTotalCost = 0;

    [Header("PlacementNames")]
    [SerializeField]
    protected string placementReborn = "Reborn";

    protected static T instance;
    private 
    protected void Awake()
    {
        if (anim == null)
            anim = GetComponent<UIAnimation>();
        instance = (T)this;

        //animResult.gameObject.SetActive(true);
        //animContinue.gameObject.SetActive(true);
    }

    protected virtual void Start()
    {
        backButton?.onClick.AddListener(() => Back());
        restartButton?.onClick.AddListener(Restart);
        rebornByFreeButton?.onClick.AddListener(RebornByFree);
        rebornByCoinButton?.onClick.AddListener(RebornByCoin);
        rebornByDiamondButton?.onClick.AddListener(RebornByDiamond);
        rebornByAdsButton?.onClick.AddListener(RebornByAds);
        nextButton?.onClick.AddListener(Next);
        shareButton?.onClick.AddListener(Share);
        rebornBySkipButton?.onClick.AddListener(SkipCountDown);
        noThanksButton?.onClick.AddListener(NoReborn);

        if (stageName)
            stageName.text = "";
        if (stageSubName)
            stageSubName.text = "";
    }

    public virtual void NoReborn()
    {
        StopCoroutine(DORebornCountDown());
        if (GameStateManager.CurrentState == GameState.GameOver)
        {
            animContinue.Hide(() =>
            {
                Debug.Log("animContinue: Hide - GameStateManager: " + GameStateManager.CurrentState);
                if (GameStateManager.CurrentState == GameState.GameOver)
                    ShowResult();
            });
        }
    }

    public virtual void ShowResult()
    {
        UIToast.ShowLoading("", 1);
        InitResultData();
        Status = UIAnimStatus.IsAnimationShow;
        if (anim.Status != UIAnimStatus.IsShow)
        {
            anim.Show(() => { 
                animResult.Show(null, () => OnShowResult()); 
        });
        }
        else
        {
            animResult.Show(null, () => OnShowResult());
        }
    }

    public virtual void ShowContinue()
    {
        Status = UIAnimStatus.IsAnimationShow;
        InitRebornData();
        anim.Show(() =>
        {
            animContinue.Show(null, () => StartCoroutine(DORebornCountDown()), null);
        });
    }

    public virtual void InitResultData()
    {
        if (stageName)
            stageName.text = "";
        if (stageSubName)
            stageSubName.text = "";
        if (shareButton)
            shareButton.gameObject.SetActive(false);
    }

    public virtual void InitRebornData()
    {
        rebornType = gameConfig.rebornType;

        rebornByFreeButton?.gameObject.SetActive(false);
        rebornByCoinButton?.gameObject.SetActive(false);
        rebornByDiamondButton?.gameObject.SetActive(false);
        rebornByAdsButton?.gameObject.SetActive(false);
        noThanksButton?.gameObject.SetActive(false);

        if (rebornType == RebornType.Continue)
            rebornByDes.text = "second change";
        else
            rebornByDes.text = "from last checkpoint";

        rebornByCountDownText.text = rebornElapsedMaxTime.ToString();

        if (rebornByInfo)
        {
            if (GameStatisticsManager.Score > 10 && currentStage.score > 50 && GameStatisticsManager.Score > currentStage.score)
            {
                rebornByInfo.text = "Wow... " + "<size=32><color=#FFFFFF>" + "NEW RECORD" + "</color></size>" + " has been set ...!";
            }
            else
            {
                rebornByInfo.text = "Try a lite bit!";
            }
        }

        if (rebornBy == RebornBy.Ads)
        {
            rebornByAdsButton?.gameObject.SetActive(true);
        }
        else if (rebornBy == RebornBy.Gold && rebornByCoinButton && rebornByCoinDes)
        {
            rebornByCoinButton.gameObject.SetActive(true);
            rebornByCoinTotalCost = (rebornCount + 1) * rebornByCoinCost;
            rebornByCoinDes.text = "-" + rebornByCoinTotalCost;
        }
        else if (rebornBy == RebornBy.Gem && rebornByDiamondButton && rebornByDiamondDes)
        {
            rebornByDiamondButton.gameObject.SetActive(true);
            rebornByDiamondCost = (rebornCount + 1) * rebornByDiamondCost;
            rebornByDiamondDes.text = "-" + rebornByDiamonTotalCost;
        }
        else
        {
            rebornByFreeButton?.gameObject.SetActive(true);
        }
    }


    protected float delta = 0;
    protected string timeStr = "";
    protected string timeStrLast = "";
    protected IEnumerator DORebornCountDown()
    {
        delta = 0;
        timeStr = "";
        timeStrLast = "";
        rebornElapsedTime = 0;
        while (rebornElapsedTime < rebornElapsedMaxTime)
        {
            if (UIManager.CurPopup != null && UIManager.CurPopup.Status != UIAnimStatus.IsHide)
            {
                //Debug.LogWarning("CurPopup: Waiting for animation DONE");
            }
            else if (UIToast.toastType == ToastType.Loading && UIToast.Status != UIAnimStatus.IsHide)
            {
                //Debug.LogWarning("UIToast: Waiting for animation DONE");
            }
            else
            {
                delta = 1 - rebornElapsedTime / rebornElapsedMaxTime;
                rebornElapsedTime += Time.deltaTime;

                if (rebornByCountDownImage)
                {
                    rebornByCountDownImage.fillAmount = delta;
                }

                if (rebornByCountDownImageDotTransform)
                {
                    rebornByCountDownImageDotTransform.SetLocalRotation2D(delta * 360f);
                }

                if (rebornByCountDownText)
                {
                    int timeCount = Mathf.FloorToInt(rebornElapsedMaxTime - rebornElapsedTime);

                    if (timeCount <= 3)
                        noThanksButton?.gameObject.SetActive(true);

                    if (timeCount >= 0)
                        timeStrLast = timeCount.ToString("#0");
                    else
                        timeStrLast = "!?";

                    if (timeStr != timeStrLast)
                    {
                        timeStr = timeStrLast;
                        rebornByCountDownText.text = timeStr;
                        UIAnimation.DoScale(rebornByCountDownText.transform, 1.2f, 0.25f, 0);
                        if (rebornElapsedTime >= 0.5f && !string.IsNullOrEmpty(timeStrLast))
                            SoundManager.Play("sfx_timer_" + (timeCount % 2));
                    }
                }
            }
            yield return null;
        }
        rebornElapsedTime = 9999;
        //NoReborn();
    }

    public abstract void OnShowResult();

    public virtual void Back(Action onDone = null)
    {
        GameStateManager.Idle(null);
        rebornCount = 0;
        Hide(() =>
        {
            onDone?.Invoke();
        });
    }

    public virtual void Restart()
    {
        GameStateManager.Restart(null);
        rebornCount = 0;
        Hide(() =>
        {
        });
    }

    public virtual void RebornByFree()
    {
        Reborn();
    }

    public virtual void RebornByCoin()
    {
        if (CoinManager.totalCoin >= rebornByCoinTotalCost)
        {
            CoinManager.Add(-rebornByCoinTotalCost);
            Reborn();
        }
    }

    public virtual void RebornByDiamond()
    {
        Debug.LogError("RebornByDiamond NOT IMPLEMENT");
    }

    public virtual void RebornByAds()
    {
        Reborn();
    }

    private void Reborn()
    {
        StopAllCoroutines();
        UIToast.Hide();
        Hide(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (rebornType == RebornType.Continue)
                    GameStateManager.RebornContinue(null);
                else
                    GameStateManager.RebornCheckPoint(null);
            });
            rebornCount++;
        });
    }

    public virtual void Next()
    {
        rebornCount = 0;
        Hide(() =>
        {
            GameStateManager.Next(null);
        });
    }

    protected void Share()
    {
        if (shareButton && shareButton.gameObject.activeInHierarchy)
        {
            UIManager.SaveScreenShot((onDone) =>
            {
                if (onDone)
                    UIManager.ShareScreenshotInGame(shareTitle, shareMessage);
                else
                    shareButton.gameObject.SetActive(false);
            });
        }
    }

    protected void SkipCountDown()
    {
        rebornElapsedTime += 1f;
    }

    protected void CheckScreenshot()
    {
        if (shareButton && shareImage)
        {
            shareImage.sprite = UIManager.SpriteScreenshotInGame;
            if (shareImage.sprite != null)
            {
                Debug.Log("CheckScreenshot SpriteScreenshotInGame");
                shareButton.gameObject.SetActive(true);
            }
            else
            {
                UIManager.SaveScreenShot((onDone) =>
                {
                    if (onDone)
                    {
                        UIManager.GetScreenShot((sprite) =>
                        {
                            if (sprite != null)
                            {
                                shareImage.sprite = sprite;
                                shareButton.gameObject.SetActive(true);
                            }
                        });
                    }
                });
            }
        }
    }

    public void Hide(Action onHideDone = null)
    {
        Status = UIAnimStatus.IsAnimationHide;
        animResult.Hide();
        animContinue.Hide();
        anim.Hide(() =>
        {
            onHideDone?.Invoke();
            Status = UIAnimStatus.IsHide;
        });
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SkipCountDown();
    }
}
