using System;
using System.Linq;
using UnityEngine;


public class GameStatisticsManager : MonoBehaviour
{
    #region GameState
    private StagesAsset stageDatas => DataManager.StagesAsset;
    private StageData currentStage => DataManager.CurrentStage;
    private ItemData currentItem => DataManager.CurrentItem;
    private UserData userData => DataManager.UserData;

    private static GameStatisticsManager instance = null;

    private void Awake()
    {
        instance = this;
        GameStateManager.OnStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState current, GameState last, object data)
    {
        if (current != GameState.LoadMain && currentStage != null)
        {
            switch (current)
            {
                case GameState.Init:
                    UIManager.DelScreenShot();
                    break;
                case GameState.Play:
                    userData.TotalPlay++;
                    currentStage.count++;
                    currentStage.totalPlay++;
                    TimePlayInGameStart = DateTime.Now;
                    
                break;
                case GameState.RebornCheckPoint:
                case GameState.RebornContinue:
                    currentStage.totalReborn++;
                    break;
                case GameState.Restart:
                    currentStage.totalRestart++;
                    break;
                case GameState.GameOver:
                    currentStage.totalFail++;
                    break;
                case GameState.Complete:
                    currentStage.totalComplete++;

                    if (currentStage.star < 3 && Stars == 3)
                    {
                        currentStage.star = Stars;
                    }
                    break;
            }
        }

        if (current == GameState.Init || current == GameState.Restart || current == GameState.RebornContinue || current == GameState.RebornCheckPoint)
        {
        }
        else if (current == GameState.GameOver || current == GameState.Complete)
        {
            TimePlayInGameEnd = (long)(DateTime.Now - TimePlayInGameStart).TotalSeconds;

            currentStage.totalTimePlay += TimePlayInGameEnd;
            if (currentStage.process < Mathf.FloorToInt(Process * 100))
                currentStage.process = Mathf.FloorToInt(Process * 100);
            if (currentStage.star < Stars)
                currentStage.star = Stars;

            if (currentStage.score < Score)
                currentStage.score = Score;
            if (currentItem.score < Score)
                currentItem.score = Score;
            if (currentStage.time <= 0 || currentStage.time > TimePlayInGameEnd)
                currentStage.time = TimePlayInGameEnd;

            userData.TotalTimePlay += TimePlayInGameEnd;
            userData.totalStar = stageDatas.list.Sum(x => x.star);
            userData.level = userData.totalStar;
        }
    }
    #endregion

    #region RewardInGame
    public static int goldEarn = 0;
    public static int gemEarn = 0;
    #endregion

    #region Score
    private static int score = -1;
    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            if (score != value)
            {
                score = value;
                OnScoreChanged?.Invoke(score);
            }
        }
    }

    public static event Action<int> OnScoreChanged = delegate { };
    #endregion

    #region Combo
    private static int combo = 0;
    public static int Combo
    {
        get
        {
            return combo;
        }
        set
        {
            if (combo != value)
            {
                combo = value;
            }
        }
    }

    public static event Action<int> OnComboChanged = delegate { };
    #endregion

    #region Process
    private static float process;
    public static float Process
    {
        get
        {
            return process;
        }
        set
        {
            if (process != value)
            {
                process = value;
                OnProcessChanged?.Invoke(process);
            }
        }
    }
    public static event Action<float> OnProcessChanged = delegate { };

    private static float totalProcess;
    public static float TotalProcess
    {
        get
        {
            return totalProcess;
        }
        set
        {
            if (totalProcess != value)
            {
                totalProcess = value;
            }
            OnTotalProcessChanged?.Invoke(totalProcess);
        }
    }
    public static event Action<float> OnTotalProcessChanged = delegate { };


    public static float ProcessToStar = 0;
    #endregion

    #region TimePlay
    public static DateTime TimePlayInGameStart { get; set; }
    public static long TimePlayInGameEnd { get; set; }
    #endregion

    #region Perfect

    private static int perfect = -1;
    public static int Perfect
    {
        get => perfect;
        set
        {
            if (perfect != value)
            {
                perfect = value;
                OnPerfectChanged?.Invoke(perfect);
            }
        }
    }
    public static event Action<int> OnPerfectChanged = delegate { };
    #endregion

    #region Stars
    private static int stars;
    public static int Stars
    {
        get
        {
            return stars;
        }
        set
        {
            if (stars != value)
            {
                stars = value;
                OnStarsChanged?.Invoke(stars);
            }
        }
    }

    public static event Action<int> OnStarsChanged = delegate { };
    #endregion


}
