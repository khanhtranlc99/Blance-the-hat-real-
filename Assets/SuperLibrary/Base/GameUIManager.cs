using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : GameManagerBase<GameUIManager>
{
    [SerializeField]
    private float waitTimeForLoadAd = 1;

    [SerializeField]
    private UIAnimation splashScreen = null;

    [SerializeField]
    private UIMainScreen mainScreen = null;
    public static UIMainScreen MainScreen => instance?.mainScreen;

    [SerializeField]
    private UIInGame inGameScreen = null;

    [SerializeField]
    private UIGameOver gameOverScreen = null;

    [SerializeField] private UISelectSkin selectSkinScreen = null;
    public static UIGameOver GameOverScreen => instance?.gameOverScreen;

    private DateTime startLoadTime = DateTime.Now;

    private GameConfig gameConfig => DataManager.GameConfig;
    private UserData userData => DataManager.UserData;

    UI asdfasfsf;

    protected override void Awake()
    {
        base.Awake();
        if (splashScreen)
            splashScreen.gameObject.SetActive(true);
        startLoadTime = DateTime.Now;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadGameData());
    }

    public IEnumerator LoadGameData()
    {
        yield return DataManager.DoLoad();

        while (userData == null)
        {
            DebugMode.Log("Load game data...");
            yield return null;
        }
        
        yield return FirebaseHelper.DoCheckStatus(null, true);
        
        if (userData.VersionInstall == 0)
            userData.VersionInstall = UIManager.BundleVersion;
        userData.VersionCurrent = UIManager.BundleVersion;


        GameStateManager.LoadMain(null);

        while ((int)(DateTime.Now - startLoadTime).TotalSeconds < waitTimeForLoadAd)
        {
            yield return null;
        }

        if (gameConfig.suggestUpdateVersion > userData.VersionCurrent)
        {
            ForeUpdate();
        }
        else
        {

            splashScreen?.Hide();
            mainScreen.Show(null, () =>
            {
                GameStateManager.Idle(null);
            });
        }

        int loadGameIn = (int)(DateTime.Now - startLoadTime).TotalSeconds;
        Debug.Log("loadGameIn: " + loadGameIn + "s");
    }

    public void ForeUpdate()
    {
        string title = "New version avaiable!";
        string body = "We are trying to improve the game quality by updating it regularly.\nPlease update new version for the best experience!";
        PopupMes.Show(title, body,
            "Update", () =>
            {
                if (!string.IsNullOrEmpty(UIManager.shareUrl))
                    Application.OpenURL(UIManager.shareUrl);
            },
            "Later", () =>
            {
                splashScreen?.Hide();
                mainScreen.Show(null, () =>
                {
                    GameStateManager.Idle(null);
                });
            });
    }


    public override void IdleGame(object data)
    {
        UILoadGame.Hide();
        mainScreen.Show(null, ()=> {
        });
        inGameScreen.Hide();
        gameOverScreen.Hide();
    }

    public override void LoadGame(object data)
    {
        LoadGameContent.PrepairDataToPlay(DataManager.CurrentStage, (done) =>
        {
            if (!done)
            {
                GameStateManager.Idle(true);
            }
        });
    }

    public override void InitGame(object data)
    {
        foreach (var i in UIManager.listPopup)
            i.Hide();
        DOTween.Kill(this);
    }

    IEnumerator WaitForLoading(Action onComplete)
    {
        while (UILoadGame.currentProcess < 1)
        {
            UILoadGame.Process();
            yield return null;
        }
        UILoadGame.Hide();
        onComplete?.Invoke();
    }

    public override void PlayGame(object data)
    {
        MusicManager.UnPause();
    }

    public override void PauseGame(object data)
    {
        MusicManager.Pause();

    


    }

    protected override void GameOver(object data)
    {
        inGameScreen.Hide();
        gameOverScreen.Show(GameState.GameOver);
    }

    protected override void CompleteGame(object data)
    {
        inGameScreen.Hide();
        gameOverScreen.Show(GameState.Complete);
    }

    protected override void ReadyGame(object data)
    {
        StartCoroutine(WaitForLoading(() =>
        {
            mainScreen.Hide();
            inGameScreen.Show();
            StartCoroutine(WaitToAutoPlay());
        }));
    }

    public override void ResumeGame(object data)
    {
        MusicManager.UnPause();
    }

    public override void RestartGame(object data)
    {
        DOVirtual.DelayedCall(0.25f, () =>
        {
            GameStateManager.Init(null);
            StartCoroutine(WaitToAutoPlay());
        });
    }

    public override void NextGame(object data)
    {
        GameStateManager.LoadGame(null);
    }

    protected override void WaitingGameOver(object data)
    {
        SoundManager.Play("sfx_crowd_oohs_0" + UnityEngine.Random.Range(1, 6));

        MusicManager.Stop(null);
        float timeWaitDie = .5f;
        //DOVirtual.Float(0.25f, 0.25f, 1, (t) => Time.timeScale = t).SetDelay(0.25f)
        //    .OnComplete(() => Time.timeScale = 1);
        DOVirtual.DelayedCall(timeWaitDie, () =>
        {
            if (GameStateManager.CurrentState == GameState.WaitGameOver)
                GameStateManager.GameOver(null);
        }).SetUpdate(false).SetId(this);
    }

    protected override void WaitingGameComplete(object data)
    {
        SoundManager.Play("sfx_crowd_applause_0" + UnityEngine.Random.Range(1, 4));

        MusicManager.Stop(null);
        float timeWaitDie = .5f;
        DOVirtual.DelayedCall(timeWaitDie, () =>
        {
            if (GameStateManager.CurrentState == GameState.WaitComplete)
                GameStateManager.Complete(null);
        }).SetUpdate(false).SetId(this);
    }

    protected override void RebornContinueGame(object data)
    {
        gameOverScreen.Hide();
        GameStateManager.Init(null);
        StartCoroutine(WaitToAutoPlay());
    }

    protected override void RebornCheckPointGame(object data)
    {
        gameOverScreen.Hide();
        GameStateManager.Init(null);
        StartCoroutine(WaitToAutoPlay());
    }
    IEnumerator WaitToAutoPlay()
    {
        var wait01s = new WaitForSeconds(0.1f);
        var wait05s = new WaitForSeconds(0.5f);
        while (GameStateManager.CurrentState != GameState.Ready)
            yield return wait01s;
        yield return wait05s;
        GameStateManager.Play(null);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.CurrentState == GameState.
                WaitGameOver ||
                GameStateManager.CurrentState == GameState.WaitComplete)
                return;

            var checkPopup = UIManager.listPopup.FirstOrDefault(x => x.Status == UIAnimStatus.IsShow);
            if (checkPopup != null)
            {
                checkPopup.Hide();
                return;
            }

            if (UIManager.listScreen.Any(x => x.IsAnimation))
                return;

            if (UIManager.listPopup.Any(x => x.IsAnimation))
                return;

            if (gameOverScreen.Status == UIAnimStatus.IsShow
                   && (GameStateManager.CurrentState == GameState.Complete || GameStateManager.CurrentState == GameState.GameOver))
            {
                GameStateManager.Idle(null);
            }
            else if (inGameScreen.Status == UIAnimStatus.IsShow)
            {
                if (GameStateManager.CurrentState == GameState.Play)
                {
                    GameStateManager.Pause(null);
                }
                else if (GameStateManager.CurrentState == GameState.Pause)
                {
                    GameStateManager.Play(null);
                }
            }
            else if (mainScreen.Status == UIAnimStatus.IsShow && GameStateManager.CurrentState == GameState.Idle)
            {
                PopupMes.Show("QUIT!?", "Do you realy want to quit!?",
                    "Cancel", null,
                    "Quit", () =>
                    {
                        DataManager.Save(true);
                        Application.Quit();
                    });
            }
        }
    }

    public void OpenPopupSelectSkins()
    {
        selectSkinScreen.Show();
    }
}