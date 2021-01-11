using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameCoreManager : GameManagerBase<GameCoreManager>
{

    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
    }

    private void OnPointerDownHandle(Vector3 p)
    {
        if (GameStateManager.CurrentState == GameState.Pause)
        {
            GameStateManager.Play(null);
            return;
        }
    }

    public override void IdleGame(object data)
    {
        Debug.Log("Game Core goto IdleGame");
    }

    public override void InitGame(object data)
    {
        Debug.Log("Game Core goto InitGame");


        /// vao game

        DOVirtual.DelayedCall(0.5f, () => GameStateManager.Ready(new MessageReadyGame { }));
    }

    public override void LoadGame(object data)
    {
        Debug.Log("Game Core goto LoadGame: " + DataManager.CurrentStage.name);
    }

    public override void NextGame(object data)
    {
        Debug.Log("Game Core goto NextGame");
    }

    public override void PauseGame(object data)
    {
        Debug.Log("Game Core goto PauseGame");
    }

    public override void PlayGame(object data)
    {
        Debug.Log("Game Core goto PlayGame");
        UIPerfectToast.instance.Show("LET'S GO!!!");
    }

    public override void RestartGame(object data)
    {
        Debug.Log("Game Core goto RestartGame");
    }

    public override void ResumeGame(object data)
    {
        Debug.Log("Game Core goto ResumeGame");
    }

    protected override void CompleteGame(object data)
    {
        Debug.Log("Game Core goto CompleteGame");
    }

    protected override void GameOver(object data)
    {

        Debug.Log("Game Core goto GameOver");
    }

    protected override void ReadyGame(object data)
    {
        Debug.Log("Game Core goto ReadyGame");

    }

    protected override void RebornCheckPointGame(object data)
    {
        Debug.Log("Game Core goto RebornCheckPointGame");
    }

    protected override void RebornContinueGame(object data)
    {
        Debug.Log("Game Core goto RebornContinueGame");
    }

    protected override void WaitingGameComplete(object data)
    {
        GameStatisticsManager.Process = 1;
    }

    protected override void WaitingGameOver(object data)
    {
        Debug.Log("Game Core goto WaitingGameOver");
    }
}

public struct MessageReadyGame
{
    public int step;
    public int stageIndex;
}