using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour
{
    [SerializeField] Text timeTxt = null;
    [SerializeField] Text stepTxt = null;
    [SerializeField] Text stageTxt = null;


    private float elapsed = 0;
    private int stepCount = 0;
    private int stepTotal = 0;
    private void Awake()
    {
        GameStateManager.OnStateChanged += GameStateManager_OnStateChanged;
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    private void GameStateManager_OnStateChanged(GameState current, GameState last, object data)
    {
        if(current == GameState.Init)
        {
            timeTxt.text = "00.00";
            stepTxt.text = "0";
            elapsed = 0;
        }
        else if(current == GameState.Ready)
        {
            var msg = (MessageReadyGame)data;
            stepTotal = msg.step;
            stepCount = 0;
            stepTxt.text = $"{stepCount}/{stepTotal}";
            stageTxt.text = msg.stageIndex.ToString();
        }
    }

    private void LateUpdate()
    {
        if(GameStateManager.CurrentState == GameState.Play)
        {
            timeTxt.text = elapsed.ToString("00.00");
            elapsed += Time.deltaTime;
        }
    }
}
